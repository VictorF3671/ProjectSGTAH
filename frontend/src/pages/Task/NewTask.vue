<template>
  <v-container class="pa-4" fluid>
    <v-card max-width="600" class="mx-auto">
      <v-card-title class="text-h5">Criar Nova Task</v-card-title>
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
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { createTask, type ITaskCreate, type ITask } from '@/api/TaskServices'

const router   = useRouter()
const route    = useRoute()


const props = defineProps<{ projectId: string }>()

// 2. Converta para number
const projectId = Number(props.projectId)
// estado do form
const formRef = ref()
const valid   = ref(false)
const form = ref({
  name: '',
  description: ''
})

// cancelar volta pra lista de tasks do projeto
function cancel() {
  router.push('/menu-template/dashboard') 
  // ou: router.push(`/menu-template/${projectId}`)
}

// submissão do form
async function submit() {
  if (!formRef.value.validate()) return

  const dto: ITaskCreate = {
    name:        form.value.name.trim(),
    description: form.value.description.trim(),
    projectId
  }

  try {
    const created: ITask = await createTask(dto)
    // redireciona para o details do projeto, ou para a lista de tasks
   router.push(`/menu-template/projects/${projectId}`)
  } catch (error) {
    console.error('Erro ao criar task:', error)
    // aqui você pode exibir um snackbar de erro…
  }
}
</script>