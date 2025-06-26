<template>
  <v-container fluid>
    <h1 class="text-h4">{{ isEdit ? 'Editar Projeto' : 'Novo Projeto' }}</h1>
    <p class="text-subtitle-1 mb-4">
      {{ isEdit 
         ? 'Altere o nome para editar este projeto'
         : 'Preencha todos os campos para criar um novo Projeto' 
      }}
    </p>

    <v-form ref="formRef" class="elevation-1 pa-4" v-model="valid" lazy-validation>
      <v-text-field
        v-model="form.name"
        variant="outlined"
        label="Nome do Projeto"
        :rules="[v => !!v || 'Nome é obrigatório']"
        required
      />

      <v-row class="pa-4" justify="end">
        <v-btn
          variant="outlined"
          color="error"
          class="mr-4"
          @click="cancel"
        >
          Cancelar
        </v-btn>
        <v-btn
          :disabled="!valid"
          color="primary"
          @click="submit"
        >
          {{ isEdit ? 'Salvar Alterações' : 'Criar Projeto' }}
        </v-btn>
      </v-row>
    </v-form>

    <v-snackbar v-model="snackbar" multi-line timeout="3000">
      {{ snackText }}
      <template #actions>
        <v-btn text color="white" @click="snackbar = false">Fechar</v-btn>
      </template>
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import {
  createProject,
  updateProject,
  getProjectById,
  type IProjectCreate,
  type IProject
} from '@/api/ProjectServices'


interface RouteParams {
  projectId?: string
}

const router = useRouter()
const route  = useRoute()

// força params a ter projectId
const params    = route.params as unknown as RouteParams
const projectId = params.projectId ? Number(params.projectId) : null
const isEdit    = projectId !== null

// estado do form
const formRef = ref()
const valid   = ref(false)
const form = ref<IProjectCreate>({
  name: ''
})

// snackbar
const snackbar = ref(false)
const snackText = ref('')

// se for edição, carrega dados
onMounted(async () => {
  if (isEdit && projectId !== null) {
    try {
      const data: IProject = await getProjectById(projectId)
      form.value.name = data.name
    } catch (e) {
      snackText.value = 'Erro ao carregar projeto'
      snackbar.value = true
    }
  }
})

function cancel() {
  // volta para o dashboard ou detalhes do projeto
  if (isEdit && projectId !== null) {
    router.push(`/menu-template/projects/${projectId}`)
  } else {
    router.push('/menu-template/dashboard')
  }
}

async function submit() {
  if (!valid.value) {
    snackText.value = 'Preencha o nome do projeto'
    snackbar.value = true
    return
  }

  try {
    if (isEdit && projectId !== null) {
      await updateProject(projectId, form.value)
    } else {
      await createProject(form.value)
    }
    // após criar/editar, vai para a lista de projetos (dashboard)
    router.push('/menu-template/dashboard')
  } catch (error: any) {
    snackText.value = error.response?.data || 'Erro ao salvar projeto'
    snackbar.value = true
  }
}
</script>