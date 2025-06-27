<template>
  <v-container class="d-flex justify-center align-center" style="height: 100vh;">
    <v-card class="pa-4" style="width: 400px; height: 400px; justify-content: center;" elevation="9">
      <v-card-title class="text-center" style=" margin-bottom: 50px;">Projeto Inga</v-card-title>
      
      <v-card-text>
        <v-form >
          <v-text-field variant="outlined" label="Username" v-model="login.username" type="text" required></v-text-field>

          <v-text-field variant="outlined" label="Senha" v-model="login.password" type="password" required></v-text-field>

          <v-btn color="primary" @click="VerifyLogin" block>Entrar</v-btn>
        </v-form>
      </v-card-text>
    </v-card>

     <v-snackbar
      v-model="snackbar"
      multi-line
    >
      {{ snacktext }}

      <template v-slot:actions>
        <v-btn
          color="red"
          variant="text"
          @click="snackbar = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-container>
</template>


<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import type { ILogin } from '@/api/UserServices';
import { authUser } from '@/api/UserServices';
const login = ref<ILogin>({
  username: '',
  password: '',
});

const router = useRouter();
const snackbar = ref(false)
const snacktext = ref("")

async function VerifyLogin() {
  if (!login.value.username || !login.value.password) {
    snacktext.value = "Preencha todos os Campos Corretamente"
    snackbar.value = true
    return;
  }
    try {
      const response = await authUser(login.value)
      if (response.token) {
      
       const token = response.token 
       localStorage.setItem('token', token);
       router.push('menu-template')
      }
    }catch(error){
      console.log(error)
      snacktext.value = "Erro ao fazer Login. Tente Novamente";
      snackbar.value = true; 
    }
}
</script>