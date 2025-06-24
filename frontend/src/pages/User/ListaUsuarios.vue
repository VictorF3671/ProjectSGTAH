<template>
  <v-container fluid>
    <v-row justify="space-between" align="center" class="pa-4" style="gap: 16px;">
      <div>
        <h1>Lista de Usuarios</h1>
        <p class="text-subtitle-1">Lista de Todos os Usuarios Cadastrados</p>
      </div>


      <div class="d-flex align-center" style="gap: 12px;">
        <v-text-field v-model="search" label="Buscar" prepend-inner-icon="mdi-magnify" dense hide-details
          variant="outlined" width="200px"></v-text-field>

        <v-btn color="primary" @click="goToNovoUsuario" height="40" style="height: 50px;">
          Novo Usuario
        </v-btn>
      </div>
    </v-row>

    <v-data-table :headers="headers" :items="usuarios" :search="search" class="mt-4 elevation-1 rounded-lg"  color="primary" outlined dense hide-default-footer></v-data-table>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router';
import type { IUsuarios } from '@/pages/Usuarios/IUsuarios'
import { getAllUsers } from '@/services/UserServices';
import { onMounted } from 'vue';
const search = ref('');
const router = useRouter()
const usuarios = ref<IUsuarios[]>([]);

const goToNovoUsuario = async () => {
  router.push('novo-usuario')
}

const headers = ref([
  { title: 'Id', key: 'id', align: "start" },
  { title: 'Nome', key: 'name', align: "start"   },
  { title: 'Email', key: 'email', align: "start"  },
  { title: 'Telefone', key: 'phoneNumber', align: "start"  },
  { title: 'Grupo', key: 'group' , align: "start" },
]);

const carregarUsuarios = async () => {
  try {
    usuarios.value = await getAllUsers();

  }catch(error){

  }
}

onMounted(() => {
  carregarUsuarios()
})


</script>