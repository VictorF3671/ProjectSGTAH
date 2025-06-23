<template> 

    <v-container fluid>
        <h1>Novo Projeto</h1>
        <p class="text-subtitle-1 mb-4">Preencha todos os campos para criar um novo Projeto</p>

        <v-form class="elevation-1 pa-4">


            <v-text-field
            variant="outlined"
            label="Nome do Projeto"
            v-model="project.name"
            > 

            </v-text-field>

            <v-row class="pa-4" style="justify-content: end;"> 
        
        <v-btn style="margin-right: 20px;" variant="outlined" color="error" @click="goToDash">
        Cancelar
        </v-btn>
        <v-btn color="primary" @click="verifyProject">
        Criar Projeto
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
import router from '@/router';
import { ref } from 'vue'
import ProjectDetails from './ProjectDetails.vue';
import { createProject, type IProjectCreate } from '@/api/ProjectServices';

const snackbar = ref(false);
const snacktext = ref("");

const project = ref<IProjectCreate>({
    name: "",
});


const verifyProject = async () => {
    if(!project.value.name){
        snacktext.value = "Preencha o campo corretamente"
        snackbar.value = true
        return;
    }
    try{
        const response = await createProject(project.value)
        if(response){
            router.push('dashboard')
        }
    }catch(error){

    }
}
const goToDash = () => {
    router.push('/menu-template/dashboard')
}

</script>