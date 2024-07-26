<script setup>
import Chat from "./components/Chat.vue";
</script>

<template>
  <div class="container">
    <div class="signup">
      <div class="logos">
        <a href="https://vitejs.dev" target="_blank">
          <img src="/vite.svg" class="logo" alt="Vite logo" />
        </a>
        <a href="https://vuejs.org/" target="_blank">
          <img src="./assets/vue.svg" class="logo vue" alt="Vue logo" />
        </a>
      </div>
      <Chat />
    </div>
    <div class="magicBackground">
      <img src="./assets//mtg.jpg" alt="" class="mtgImage" />
    </div>
  </div>
</template>
