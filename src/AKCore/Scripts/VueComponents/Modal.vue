<template>
  <transition name="modal">
    <div
      v-if="showModal"
      class="modal-wrapper"
      :class="{ notransition: notransition }"
    >
      <div class="modal show" @click="modalClose">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <button type="button" class="close" @click.prevent="close">
                &times;
              </button>
              <h4 class="modal-title">{{ header }}</h4>
            </div>
            <slot name="body"></slot>
            <slot name="footer"></slot>
          </div>
        </div>
      </div>
      <div class="modal-backdrop fade in"></div>
    </div>
  </transition>
</template>
<script>
export default {
  props: ["showModal", "header", "notransition"],
  methods: {
    modalClose() {
      if (event.target.classList.contains("modal")) {
        this.$emit("close");
      }
    },
    close() {
      this.$emit("close");
    }
  }
};
</script>

<style lang="scss" scoped>
.modal-enter,
.modal-leave-active {
  opacity: 0;
}

.modal-enter .modal-dialog,
.modal-leave-active .modal-dialog {
  transform: translateY(-30%);
}
.modal-wrapper {
  transition: all 0.3s ease;
  &.notransition {
    transition: none;
  }
}
.modal-dialog {
  max-height: calc(100vh - 30px);
}
</style>
