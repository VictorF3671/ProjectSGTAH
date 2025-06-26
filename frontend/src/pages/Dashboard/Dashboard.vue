<template>
    <v-container fluid>
        <v-row align="center" justify="space-between">
            <v-col cols="auto">
                <h1 class="pa-4">Meus Projetos</h1>
            </v-col>
            <v-col cols="auto">
                <v-btn color="primary" @click="goToNewProject">
                    Novo Projeto
                </v-btn>
            </v-col>
        </v-row>
        <v-row>
            <v-col v-for="project in projects" :key="project.id" cols="12" sm="6" md="4">
                <v-card class="ma-4 elevation-2">
                    <v-card-title class="text-h6">{{ project.name }}</v-card-title>
                    <v-card-actions>
                        <v-btn :to="{ name: 'ProjectDetails', params: { projectId: project.id } }">
                            Ver detalhes
                        </v-btn>
                        <v-btn icon color="primary" @click="editProject(project.id)">
                            <v-icon>mdi-pencil</v-icon>
                        </v-btn>
                        <v-btn icon color="red" @click="confirmDelete(project.id)">
                            <v-icon>mdi-delete</v-icon>
                        </v-btn>

                        <v-btn icon color="primary" :to="{ name: 'NewTask', params: { projectId: project.id } }">
                            <v-icon>mdi-plus</v-icon>
                        </v-btn>
                        <v-spacer />
                    </v-card-actions>
                </v-card>
            </v-col>
        </v-row>

        <DialogConfirm v-model="confirmVisible" :title="title" :message="message" :confirm-text="confirmText"
            cancel-text="Não" @confirm="callDeleteProduct" />
    </v-container>

</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'

import { getAllProject, deleteProject } from '@/api/ProjectServices'
import type { IProject } from '@/api/ProjectServices'
import router from '@/router'

const confirmVisible = ref(false);
const title = ref('')
const message = ref('')
const confirmText = ref('')

const projects = ref<IProject[]>([])
let pendingDeleteId: number;

const loadProjects = async () => {
    try {
        projects.value = await getAllProject();

    } catch (error) {

    }
}
onMounted(() => {
    loadProjects()
})

const goToNewProject = () => {
    router.push('/menu-template/projects/new')
}


function confirmDelete(id: number) {
    pendingDeleteId = id;
    title.value = 'Confirmar Exclusão';
    message.value = 'Deseja realmente excluir o project?';
    confirmText.value = 'Sim, excluir';
    confirmVisible.value = true;

}

function editProject(projectId: number) {
    router.push(`/menu-template/projects/new/${projectId}`)
}

const callDeleteProduct = async () => {
    if (pendingDeleteId) {
        try {
            const response = await deleteProject(pendingDeleteId);
            console.log(response)
            pendingDeleteId = 0;
            loadProjects()
        } catch (error) {
        }
    }
}
</script>