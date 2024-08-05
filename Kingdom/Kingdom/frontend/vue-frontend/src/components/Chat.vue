<template>
  <div v-if="!roomCreated" class="">
    <div class="room flex flex-col">
      <input
        class="border-2"
        placeholder="Enter room name"
        v-model="roomName"
      />
      <input
        class="border-2"
        type="password"
        placeholder="Enter room password"
        v-model="roomPassword"
      />
      <p>Number of Players</p>
      <input
        class="border-2"
        type="number"
        placeholder="Number of Players"
        v-model="numberOfPlayers"
      />
      <button class="button bg-blue-700" @click="createRoom">
        Create Room
      </button>
    </div>
    <div class="room">
      <input
        class="border-2"
        type="password"
        placeholder="Enter room name"
        v-model="joinedRoomName"
      />
      <input
        class="border-2"
        type="password"
        placeholder="Enter room password"
        v-model="joinedRoomPassword"
      />
      <button class="button bg-green-700" @click="joinRoom">Join Room</button>
    </div>
  </div>
  <div class="room"></div>
  <div v-if="roomCreated">
    <Room
      :roomName="roomName"
      :playerCount="playerCount"
      :numberOfPlayers="numberOfPlayers"
      :role="role"
    />
  </div>
</template>

<script setup>
import * as signalR from "@microsoft/signalr";
import Room from "../components/Room.vue";
import { onMounted, ref } from "vue";

const connection = ref(null);
const roomName = ref("");
const roomPassword = ref("");
const joinedRoomName = ref("");
const joinedRoomPassword = ref("");
const roomCreated = ref(false);
const messages = ref([]);
const roomId = ref(0);
const playerCount = ref(0);
const numberOfPlayers = ref(0);
const role = ref("");

onMounted(() => {
  connection.value = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7090/chatHub")
    .build();

  connection.value
    .start()
    .then(() => {
      console.log("Connection started");
    })
    .catch((err) => {
      console.error("Error while starting connection: " + err);
    });

  connection.value.on("ReceiveMessage", (user, message) => {
    messages.value.push(`${user}: ${message}`);
  });

  connection.value.on("RoomCreated", (room) => {
    console.log(room, "RoomCreated");
    roomCreated.value = true;
    roomName.value = room.name;
    roomId.value = room.roomId;
    numberOfPlayers.value = room.numberOfPlayers;
    playerCount.value = room.roomParticipants;
  });

  connection.value.on("JoinedRoom", (room) => {
    console.log(room, "JoinedRoom");
    roomCreated.value = true;
    roomName.value = room.name;
    numberOfPlayers.value = room.numberOfPlayers;
    roomId.value = room.roomId;
    playerCount.value = room.roomParticipants;
    console.log(roomName.value);
  });

  connection.value.on("ReceiveRole", (assignedRole) => {
    role.value = assignedRole;
    console.log("Assigned role:", assignedRole);
  });

  connection.value.on("Error", (error) => {
    alert(error);
  });
});

const createRoom = () => {
  console.log(roomName.value);
  console.log(roomPassword.value);
  console.log(numberOfPlayers.value);
  connection.value.invoke(
    "CreateRoom",
    roomName.value,
    roomPassword.value,
    numberOfPlayers.value
  );
};

const joinRoom = () => {
  connection.value.invoke(
    "JoinRoom",
    joinedRoomName.value,
    joinedRoomPassword.value
  );
};
</script>
