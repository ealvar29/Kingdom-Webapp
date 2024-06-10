<script setup>
import { defineProps } from "vue";
import * as signalR from "@microsoft/signalr";

const props = defineProps({
  roomName: String,
  numOfPeople: Number,
});

let connection = null;
let messages = [];
let roomName = props.roomName;
let roomPassword = "";
let message = "";
let roomCreated = false;
let numOfPeople = props.numOfPeople;
</script>

<template>
  <div class="room">
    <p>You are in the room {{ roomName }}</p>
    <p>{{ numOfPeople }}</p>
    <button class="button bg-red-700">Get Role</button>
  </div>
</template>
