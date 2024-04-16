<template>
  <div>
    <input v-model="username" placeholder="Enter name" />
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
      username: "",
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
  },
  methods: {
    sendMessage() {
      this.connection.invoke("SendMessage", this.username, this.message);
    },
  },
};
</script>
