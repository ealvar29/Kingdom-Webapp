<template>
  <div>
    <input v-model="roomName" placeholder="Enter room name" />
    <input
      v-model="roomPassword"
      type="password"
      placeholder="Enter room password"
    />
    <button @click="createRoom">Create Room</button>
    <button @click="joinRoom">Join Room</button>

    <input v-model="message" placeholder="Enter message" />
    <button @click="sendMessage">Send Message</button>
    <ul>
      <li v-for="msg in messages" :key="msg">{{ msg }}</li>
    </ul>
  </div>
</template>

<script>
import * as signalR from "@microsoft/signalr";

export default {
  data() {
    return {
      connection: null,
      messages: [],
      roomName: "",
      roomPassword: "",
      message: "",
    };
  },
  mounted() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7090/chatHub")
      .build();

    this.connection
      .start()
      .catch((err) => console.error("Error while starting connection: " + err));

    this.connection.on("ReceiveMessage", (user, message) => {
      this.messages.push(`${user}: ${message}`);
    });

    this.connection.on("RoomCreated", (room) => {
      alert(`Created and joined room: ${room}`);
    });

    this.connection.on("JoinedRoom", (room) => {
      alert(`Joined room: ${room}`);
    });

    this.connection.on("Error", (error) => {
      alert(error);
    });
  },
  methods: {
    createRoom() {
      this.connection.invoke("CreateRoom", this.roomName, this.roomPassword);
    },
    joinRoom() {
      this.connection.invoke("JoinRoom", this.roomName, this.roomPassword);
    },
    sendMessage() {
      this.connection.invoke("SendMessageToRoom", this.roomName, this.message);
    },
  },
};
</script>
