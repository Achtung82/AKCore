﻿<template>
  <div class="row">
    <div class="col-sm-2 image" @click="pickImage">
      <img class="album-img" :src="album.image">
    </div>
    <div class="col-sm-4 name">
      <input
        ref="inputelement"
        v-if="editName"
        type="text"
        class="name-input"
        v-model="name"
        @keyup.enter="onInputBlur"
        @blur="onInputBlur"
      >
      <span class="album-name" v-if="!editName" @click="showInput">{{name}}</span>
    </div>
    <div class="col-sm-2 category">
      <select class="form-control" v-model="albumCategory" @change="onCategoryChange">
        <option v-for="cat in albumCategories" :key="cat">{{cat}}</option>
      </select>
    </div>
    <div class="col-sm-1 actions">
      <a href="#" class="del-album btn glyphicon glyphicon-remove" @click.prevent="deleteAlbum"></a>
    </div>
    <div class="col-sm-3 tracks" @click="uploadTracks">
      <span class="tracks-info">
        {{tracks}} spår uppladdade.
        <br>Klicka här för att hantera.
      </span>
    </div>
  </div>
</template>
<script>
import Constants from "../../constants";

export default {
  props: ["album"],
  data() {
    return {
      editName: false,
      name: "",
      albumCategory: ""
    };
  },
  methods: {
    showInput() {
      this.editName = true;
      this.$nextTick(() => this.$refs.inputelement.focus());
    },
    onInputBlur() {
      if (this.editName) {
        this.editName = false;
        if (this.album.name !== this.name) {
          this.$emit("name", this.name, this.album.id);
        }
      }
    },
    onCategoryChange() {
      this.$emit("category", this.albumCategory, this.album.id);
    },
    uploadTracks() {
      this.$emit("tracks", this.album);
    },
    deleteAlbum() {
      if (
        window.confirm(
          "är du säker på att du vill ta bort album med namn " +
            this.album.name +
            " ?"
        )
      ) {
        this.$emit("delete", this.album.id);
      }
    },
    pickImage() {
      this.$emit("image", this.album.id);
    }
  },
  computed: {
    tracks() {
      return this.album.tracks.length;
    },
    albumCategories() {
      return Constants.ALBUMCATEGORIES;
    }
  },
  created() {
    this.name = this.album.name;
    this.albumCategory = this.album.category ? this.album.category : "Övrigt";
  }
};
</script>
<style lang="scss" scoped>
@import "../../../Styles/variables.scss";
.row:hover {
  background-color: #1a0000;
}

.image,
.name,
.actions,
.category,
.tracks {
  height: 100px;
  vertical-align: middle;
}

.actions,
.image,
.category,
.tracks {
  display: flex;
  align-items: center;
}

.tracks {
  cursor: pointer;
}

.name {
  font-size: 20px;
  display: flex;
  align-items: center;

  .name-input {
    background-color: transparent;
    border: 0;
    outline: 0;

    &:focus {
      background: $akwhite;
      color: #000;
    }
  }
}

.del-album {
  font-size: 20px;
  color: red;
}

.album-img {
  height: 100px;
  max-width: 100px;
}
</style>
