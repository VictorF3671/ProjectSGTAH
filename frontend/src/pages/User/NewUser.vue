<template>
    <v-container fluid class="pa-4">
        <h1>Novo Usuario</h1>
        <p >Preencha os campos necessários para criar um novo usuário</p>

        
        <v-form class="pa-4 elevation-1" style="margin-top: 30px;"> 
        <v-row >
            <v-col>
            <v-text-field variant="outlined" type="text" label="Nome" required v-model="user.username">
            </v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col>
            <v-text-field variant="outlined" type="password" label="Senha" required v-model="user.password">
            </v-text-field>
            </v-col>
        </v-row>

        <v-row class="pa-4" style="justify-content: end;"> 
        
        <v-btn style="margin-right: 20px;" variant="outlined" color="error" @click="navegarLista">
        Cancelar
        </v-btn>
        <v-btn color="primary" @click="verifyCreate">
        Criar Usuario
        </v-btn>
        </v-row>
        </v-form>

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
import { createUser, type IUserCreate } from '@/api/UserServices';

const snackbar = ref(false);
const snacktext = ref("");
const router = useRouter() 


const items = [
    { title: "Vendedor", key: 1 },
    { title: "Administrador", key: 2 }
]
const user = ref<IUserCreate>({
    username: "",
    password: "",
});
function navegarLista(){
    router.push('dashboard')
}
async function verifyCreate() {
     if(!user.value.username || !user.value.password ){
         snacktext.value = "Preencha todos os Campos Corretamente"
         snackbar.value = true;
         return;
     }
    try{
        const response = await createUser(user.value)
        if(response.success){
            snacktext.value = "Usuario Criado com Sucesso"
            snackbar.value = true;
            
            setTimeout(()=> {
                navegarLista()
            },3000)

        }
        }catch(error){
            snacktext.value = "Erro ao cadastrar usuario. Tente novamente"
            snackbar.value = true;

    }

}
</script>