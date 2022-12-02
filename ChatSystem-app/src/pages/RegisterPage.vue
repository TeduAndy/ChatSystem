<template>
  <div class="row justify-center items-center window-height">
    <q-card class="my-card col-11 col-md-4">
      <q-card-section class="text-center text-h6">
        Register
      </q-card-section>
      <q-card-section>
        <q-input color="blue-grey-3" v-model="state.userName" label="UserName">
          <template v-slot:prepend>
            <span class="material-icons">account_circle</span>
          </template>
        </q-input>
      </q-card-section>
      <q-card-section>
        <q-input color="blue-grey-3" type="password" v-model="state.passWord" label="PassWord">
          <template v-slot:prepend>
            <span class="material-icons">vpn_key</span>
          </template>
        </q-input>
      </q-card-section>
      <q-card-section>
        <q-input color="blue-grey-3" type="password" v-model="state.checkPassword" label="Check PassWord">
          <template v-slot:prepend>
            <span class="material-icons">vpn_key</span>
          </template>
        </q-input>
      </q-card-section>
      <q-card-section>
        <q-input color="blue-grey-3" type="email" v-model="state.eMail" label="E-Mail">
          <template v-slot:prepend>
            <span class="material-icons">mail</span>
          </template>
        </q-input>
      </q-card-section>
      <q-card-section class="q-mt-sm">
        <q-btn color="white" text-color="black" label="Sign up" class="full-width" @click="registerFunc()"/>
      </q-card-section>
      <q-card-section class="q-mt-sm text-center">
        <div>If you register? you can sign in</div>
      </q-card-section>
    </q-card>
  </div>
</template>
<script setup>
import { reactive } from 'vue';
import { useQuasar } from 'quasar'

const $q = useQuasar()

// 變數區塊
let state = reactive({
  userName: '',
  passWord: '',
  checkPassword: '',
  eMail: '',
})

// 邏輯處理
const registerFunc =async ()=>{

  // 驗證密碼跟確認密碼是否正確
  if(state.passWord != state.checkPassword){
    $q.dialog({
        title: 'Warning',
        message: 'PassWord and CheckPassWord is different!!!',
        style:"text-align: center;",
        stackButtons: true,
        persistent: true
      }).onOk(async () => {
        state.passWord = ''
        state.checkPassword = ''
      })
  }
}
</script>
<style lang="scss">
</style>

