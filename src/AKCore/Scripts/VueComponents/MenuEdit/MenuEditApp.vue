﻿<template>
  <div id="menu-edit-app">
    <div class="row">
      <div class="col-md-6">
        <button
          type="button"
          class="btn btn-primary"
          @click.prevent="toggleCreateMenu"
        >Lägg till en toppmeny</button>
      </div>
    </div>
    <div class="row edit-form" v-if="showAddMenu">
      <div class="col-md-6">
        <form action="/MenuEdit/AddTopMenu" @submit.prevent="createMenu" method="post">
          <div class="alert alert-danger" style="display: none;"></div>
          <div class="alert alert-success" style="display: none;">Meny sparad</div>
          <div class="form-group">
            <label for="name">Menytext:</label>
            <input type="text" class="form-control" name="name" placeholder="Menytext">
          </div>
          <div class="form-group">
            <label for="slug">Menylänk:</label>
            <select name="pageId" class="form-control">
              <option>Välj en sida</option>
              <option :value="page.id" v-for="page in pages" :key="page.id">{{page.name}}</option>
            </select>
          </div>
          <div class="checkbox">
            <label>
              <input name="loggedIn" type="checkbox"> Kräv inloggning
            </label>
          </div>
          <div class="checkbox">
            <label>
              <input name="balett" class="balett" type="checkbox"> Visa enbart för balett
            </label>
          </div>
          <button type="submit" class="btn btn-default">Skapa</button>
        </form>
      </div>
    </div>
    <menu-list :menus="menus" @update="loadMenus" @edit="editMenu"></menu-list>
    <menu-edit-modal :show-modal="showEditModal" :pages="pages" :parent-id="parentId" :menu="editedMenu" @update="loadMenus" @close="closeEditMenu"></menu-edit-modal>
  </div>
</template>
<script>
import ApiService from "../../services/apiservice";
import MenuList from "./MenuList";
import MenuEditModal from "./MenuEditModal";

export default {
  components: {
    MenuList,
    MenuEditModal
  },
  data() {
    return {
      menus: null,
      showAddMenu: false,
      pages: null,
      showEditModal: false,
      editedMenu: null,
      parentId: null
    };
  },
  methods: {
    editMenu(menu, parent) {
      this.showEditModal = true;
      this.editedMenu = menu;
      if(parent) {
        this.parentId = parent.id;
      }
    },
    loadMenus() {
      ApiService.get("/MenuEdit/MenuListData", null, res => {
        this.menus = res.menus;
        this.pages = res.pages;
      });
    },
    toggleCreateMenu() {
      this.showAddMenu = !this.showAddMenu;
    },
    createMenu(event) {
      const form = $(event.target);
      ApiService.defaultFormSend(form, null, null, () => {
        this.loadMenus();
        this.showAddMenu = false;
        form.trigger("reset");
      });
    },
    closeEditMenu() {
      this.showEditModal = false;
      this.editedMenu = null;
      this.parentId = null;
    }
  },
  created() {
    this.loadMenus();
  }
};
</script>
<style lang="scss" scoped>
.edit-form {
  padding-top: 10px;
}

.menualerts {
  margin-top: 20px;
}
</style>
