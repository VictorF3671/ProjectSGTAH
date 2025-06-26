<template>
  <v-container class="pa-4" fluid>
    <v-card max-width="600" class="mx-auto">
      <v-card-title class="text-h5">
        {{ isEdit ? 'Editar Task' : 'Criar Nova Task' }}
      </v-card-title>
      <v-card-text>
        <v-form ref="formRef" v-model="valid" lazy-validation>
          <v-text-field
            v-model="form.name"
            label="Nome da Task"
            variant="outlined"
            :rules="[v => !!v || 'O nome é obrigatório']"
            required
          />
          <v-text-field
            v-model="form.description"
            label="Descrição (opcional)"
            variant="outlined"
            textarea
            rows="3"
          />
        </v-form>
      </v-card-text>
      <v-card-actions class="justify-end">
        <v-btn text @click="cancel">Cancelar</v-btn>
        <v-btn :disabled="!valid" color="primary" @click="submit">
          Salvar
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import {
  createTask,
  updateTask,
  getTaskById,
  type ITaskCreate,
  type ITask
} from '@/api/TaskServices'


interface RouteParams {
  projectId: string
  taskId?: string
}

const router = useRouter()
const route  = useRoute()

const params     = route.params as unknown as RouteParams
const projectId  = Number(params.projectId)
const taskId     = params.taskId ? Number(params.taskId) : null
const isEdit     = taskId !== null

const formRef = ref()
const valid   = ref(false)
const form = ref({
  name:        '',
  description: ''
})

onMounted(async () => {
  if (isEdit && taskId !== null) {
    try {
      const data: ITask = await getTaskById(taskId)
      form.value.name        = data.name
      form.value.description = data.description
    } catch (e) {
      console.error('Erro ao buscar task:', e)
    }
  }
})

function cancel() {
  // volta para detalhes do projeto
  router.push(`/menu-template/projects/${projectId}`)
  // ou se sua rota de detalhes for /menu-template/projects/:projectId:
  // router.push(`/menu-template/projects/${projectId}`)
}

async function submit() {
  if (!formRef.value.validate()) return

  const dto: ITaskCreate = {
    name:        form.value.name.trim(),
    description: form.value.description.trim(),
    projectId
  }

  try {
    if (isEdit && taskId !== null) {
      await updateTask(taskId, dto)
    } else {
      await createTask(dto)
    }
    router.push(`/menu-template/projects/${projectId}`)
  } catch (error: any) {
    console.error('Erro ao salvar task:', error)
    // exiba snackbar de erro aqui
  }
}
</script>