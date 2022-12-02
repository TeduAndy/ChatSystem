<template>
  <div class="row justify-center items-center window-height">
    <q-card class="my-card col-11 col-md-4">
      <q-card-section class="text-center">
        <h5>System Chat</h5>
      </q-card-section>
      <q-card-section>
        <q-input color="blue-grey-3" v-model="state.username" label="UserName">
          <template v-slot:prepend>
            <span class="material-icons">account_circle</span>
          </template>
        </q-input>
      </q-card-section>
      <q-card-section>
        <q-input color="blue-grey-3" v-model="state.password" label="PassWord">
          <template v-slot:prepend>
            <span class="material-icons">vpn_key</span>
          </template>
        </q-input>
      </q-card-section>
      <q-card-section class="q-mt-sm">
        <q-btn color="white" text-color="black" label="Sign in" class="full-width"/>
      </q-card-section>
    </q-card>
  </div>
</template>
<script setup>
import { reactive } from 'vue';

// 變數區塊
let state = reactive({
  username: '',
  password: ''
})

</script>
<style lang="scss">
</style>
