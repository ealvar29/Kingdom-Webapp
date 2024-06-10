<template>
  <div v-if="!roomCreated" class="">
    <div class="room flex flex-col">
      <input class="border-2" placeholder="Enter room name" />
      <input
        class="border-2"
        type="password"
        placeholder="Enter room password"
      />
      <button class="button bg-blue-700" @click="createRoom">
        Create Room
      </button>
    </div>
    <div class="room">
      <input class="border-2" type="password" placeholder="Enter room name" />
      <input
        class="border-2"
        type="password"
        placeholder="Enter room password"
      />
      <button class="button bg-green-700" @click="joinRoom">Join Room</button>
    </div>
  </div>
  <div class="room">
    <p class="text-red-500">Room Created? : {{ roomCreated }}</p>
    <p>Number of people: {{ numOfPeople }}</p>
    <p>Room Id: {{ roomId }}</p>
  </div>
  <div v-if="roomCreated">
    <Room :roomName="roomName" :numOfPeople="numOfPeople" />
  </div>
</template>
<script setup>
import * as signalR from "@microsoft/signalr";
import Room from "../components/Room.vue";
import { onMounted, ref } from "vue";

const connection = ref(null);
const roomName = ref("");
const roomPassword = ref("");
const roomCreated = ref(false);
const messages = ref([]);
const message = ref("");
const roomId = ref(0);
const numOfPeople = ref(0);

onMounted(() => {
  connection.value = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7090/chatHub")
    .build();

  connection.value
    .start()
    .catch((err) => console.error("Error while starting connection: " + err));

  connection.value.on("ReceiveMessage", (user, message) => {
    messages.value.push(`${user}: ${message}`);
  });

  connection.value.on("RoomCreated", (room) => {
    console.log(room);
    roomCreated.value = true;
    roomName.value = room.name;
    roomId.value = room.roomId;
    numOfPeople.value = room.roomParticipants;
    console.log(roomName.value);
  });

  connection.value.on("JoinedRoom", (room) => {
    console.log(room);
    roomCreated.value = true;
    roomName.value = room.name;
    roomId.value = room.roomId;
    numOfPeople.value = room.roomParticipants;
    console.log(roomName.value);
  });

  connection.value.on("Error", (error) => {
    alert(error);
  });
});

const createRoom = () => {
  connection.value.invoke("CreateRoom", roomName.value, roomPassword.value);
};

const joinRoom = () => {
  connection.value.invoke("JoinRoom", roomName.value, roomPassword.value);
};

const sendMessage = () => {
  connection.value.invoke("SendMessageToRoom", roomName.value, message.value);
};
</script>
