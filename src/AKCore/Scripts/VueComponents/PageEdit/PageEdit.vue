﻿<template>
  <div id="pageedit-app" v-if="pageModel">
    <div class="row">
      <form method="post" @submit.prevent="save">
        <div class="alert alert-danger" style="display: none;"></div>
        <div class="alert alert-success" style="display: none;"></div>
        <page-meta v-model="pageModel"></page-meta>
      </form>
    </div>
    <add-widget @add="widgetAdd"></add-widget>
  </div>
</template>
<script>
import Constants from "../../constants";
import PageMeta from "./PageMeta";
import AddWidget from "./AddWidget";
import ApiService from "../../services/apiservice";

export default {
  components: {
    PageMeta,
    AddWidget
  },
  data() {
    return {
      pageModel: null
    };
  },
  created() {
    const self = this;
    $.ajax({
      url: window.location.href + "/Model",
      type: "GET",
      success: function(res) {
        self.pageModel = res;
      }
    });
  },
  methods: {
    widgetAdd(type) {
      console.log(type);
    },
    save(e) {
      const self = this;
      const success = $(".alert-success");
      const error = $(".alert-danger");

      ApiService.postByObject(
        window.location.href,
        this.pageModel,
        error,
        success,
        null
      );
    }
  }
};
</script>
<style lang="scss"></style>