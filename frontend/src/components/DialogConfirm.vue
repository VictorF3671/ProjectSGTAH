<template>
  <v-dialog v-model="visible" attach="body" max-width="400">
    <v-card>
     <v-card-title class="text-h6">{{ computedTitle }}</v-card-title>
  <v-card-text>{{ computedMessage }}</v-card-text>
  <v-card-actions class="justify-end">
    <v-btn text @click="onCancel">{{ computedCancelText }}</v-btn>
    <v-btn color="primary" text @click="onConfirm">{{ computedConfirmText }}</v-btn>
  </v-card-actions>
    </v-card>
  </v-dialog>

</template>

<script setup lang="ts">
import { defineProps, defineEmits, ref, watch, computed } from 'vue';

const props = defineProps<{
  modelValue: boolean;
  title?: string;
  message?: string;
  confirmText?: string;
  cancelText?: string;
}>();

const emit = defineEmits<{
  (e: 'update:modelValue', v: boolean): void;
  (e: 'confirm'): void;
  (e: 'cancel'): void;
}>();

const visible = ref(props.modelValue);

watch(() => props.modelValue, v => (visible.value = v));
watch(visible, v => emit('update:modelValue', v));

const computedTitle = computed(() => props.title ?? 'Confirmação');
const computedMessage = computed(() => props.message ?? 'Deseja prosseguir?');
const computedConfirmText = computed(() => props.confirmText ?? 'Confirmar');
const computedCancelText = computed(() => props.cancelText ?? 'Cancelar');

function onConfirm() {
  emit('confirm');
  visible.value = false;
}

function onCancel() {
  emit('cancel');
  visible.value = false;
}
</script>