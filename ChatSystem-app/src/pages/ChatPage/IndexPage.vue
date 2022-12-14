<template>
  {{ hubUrl }}
  <!-- <div class="text-center q-mt-xl">
    <div class="row">
      <div class="col-2">User</div>
      <div class="col-4"><input type="text" id="userInput" /></div>
    </div>
    <div class="row">
      <div class="col-2">Message</div>
      <div class="col-4"><input type="text" id="messageInput" /></div>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row">
      <div class="col-6">
        <input type="button" id="sendButton" value="Send Message" />
      </div>
    </div>
  </div>
  <div class="row">
    <div class="col-12">
      <hr />
    </div>
  </div>
  <div class="row">
    <div class="col-6">
      <ul id="messagesList"></ul>
    </div>
  </div> -->
  <div>123</div>
</template>

<script setup>
import { onMounted } from "vue";
import { HubConnectionBuilder } from "@microsoft/signalr";
const hubUrl =
  import.meta.env.MODE == "development" ? "http://127.0.0.1/chatHub" : "";
const conn = new HubConnectionBuilder().withUrl(hubUrl).build();

let Event = {};
Event.ReceiveMessage = {
  name: "ReceiveMessage",
  handler: (user, message) => {
    let li = document.createElement("li");
    document.querySelector("#messagesList").appendChild(li);
    li.textContent = `${user} says ${message}`;
  },
};

// conn.on(Events.ReceiveMessage.name, Events.ReceiveMessage.handler);
Object.keys(Events).forEach((k) => {
  const { name, handler } = Events[k];
  conn.on(name, handler);
});

// try {
//   await conn.start();
//   // document.querySelector("#sendButton").disabled = false;
// } catch (err) {
//   return console.error(err.toString());
// }

// 這邊註釋修改成上面
// connection.start().then(function () {
//     document.getElementById("sendButton").disabled = false;
// }).catch(function (err) {
//     return console.error(err.toString());
// });

// document
//   .querySelector("#sendButton")
//   .addEventListener("click", function (event) {
//     let user = document.querySelector("#userInput").value;
//     let message = document.querySelector("#messageInput").value;
//     // conn.invoke('SendMessage', user, message).catch(function (err) {
//     //     return console.error(err.toString());
//     // });
//     try {
//       conn.invoke("SendMessage", user, message);
//     } catch (err) {
//       return console.error(err.toString());
//     }

//     event.preventDefault();
//   });
</script>

<style lang="scss" scoped></style>
