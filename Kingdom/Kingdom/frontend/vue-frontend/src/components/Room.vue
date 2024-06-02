<script setup>
import { defineProps } from "vue";
import * as signalR from "@microsoft/signalr";

const props = defineProps({
  roomName: String,
});

console.log(props.roomName);

let connection = null;
let messages = [];
let roomName = props.roomName;
let roomPassword = "";
let message = "";
let roomCreated = false;
</script>

<template>
  <div>
    <p>You are in the room {{ roomName }}</p>
  </div>
</template>
