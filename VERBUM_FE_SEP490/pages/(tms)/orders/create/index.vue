<script setup lang="ts">
import { Check, Circle, Dot } from 'lucide-vue-next'
import { toTypedSchema } from '@vee-validate/zod'
import * as z from 'zod'
import { h, ref } from 'vue'
import {
  Stepper,
  StepperDescription,
  StepperItem,
  StepperSeparator,
  StepperTitle,
  StepperTrigger
} from '@/components/ui/stepper'
import { Form } from '@/components/ui/form'
import { Button } from '@/components/ui/button'
import { format } from 'date-fns'


const formSchema = [
  z.object({
    sourceLanguageId: z.string(),
    targetLanguageIdList: z.string(),
    translationFileURL: z.string(),
    dueDate: z.coerce.date()
  }),
  z.object({
    hasTranslateService: z.boolean().default(false),
    hasEditService: z.boolean().default(false),
    hasEvaluateService: z.boolean().default(false),
  }),
  z.object({
    reference: z.string(),
    referenceFileURLs: z.string(),
    // discountId: z.string()
  })
]

const stepIndex = ref(1)
const steps = [
  {
    step: 1,
    title: 'Order Information',
    description: 'Provides your files and languages you want to translate'
  },
  {
    step: 2,
    title: 'Services',
    description: 'Choose which services you want to include in the order'
  },
  {
    step: 3,
    title: 'References',
    description:
      'Provides futher details like notes, references, instructions or any special requests'
  }
]

async function onSubmit(values: any) {
  // Convert the targetLanguageIdList and referenceFileURLs to arrays
  const payload = {
    orderName: 'Order Name',
    ...values,
    sourceLanguageId: values.sourceLanguageId,
    targetLanguageIdList: values.targetLanguageIdList.split(',').map((id: string) => id.trim()),
    translationFileURL: values.translationFileURL.split(',').map((id: string) => id.trim()),
    dueDate: format(values.dueDate, "yyyy-MM-dd'T'HH:mm:ss"),
    hasTranslateService: values.hasTranslateService ?? false,
    hasEditService: values.hasEditService ?? false,
    hasEvaluateService: values.hasEvaluateService ?? false,
    reference: values.reference,
    referenceFileURLs: values.referenceFileURLs.split(',').map((url: string) => url.trim()),
    discountId: null
  }

  try {
    // Send the payload to the backend using a POST request
    const { data, error } = await $fetch('http://localhost:8000/api/order/add', {
      method: 'POST',
      body: payload, // Pass the payload as the body
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiOTI2Y2ZiNzEtZDlmNC00ZDIxLWE4N2EtNDE5MWJkM2I3YzU5IiwiZW1haWwiOiJsYW1waHVuZzIxMy5waHVjQGdtYWlsLmNvbSIsIm5hbWUiOiJMw6JtIFBow7luZyIsInN0YXR1cyI6IkFDVElWRSIsInJvbGUiOiJDTElFTlQiLCJleHAiOjE3Mjg5NzgzNjAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjgwMDAiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAwIn0.uQpVard8EFH_4N6XNi90PLLKSJNkgcLgZYApm8dghe8'
      },
    })

    if (error) {
      console.error('Error submitting form:', error)
    } else {
      console.log('Form submitted successfully:', data)
    }
  } catch (err) {
    console.error('Request failed:', err)
  }
}

</script>

<template>
  <Form
    v-slot="{ meta, values, validate }"
    as=""
    keep-values
    :validation-schema="toTypedSchema(formSchema[stepIndex - 1])"
  >
    <Stepper
      v-slot="{ isNextDisabled, isPrevDisabled, nextStep, prevStep }"
      v-model="stepIndex"
      orientation="vertical"
      class="block"
    >
      <form
        class="flex justify-start max-w-screen-lg mx-auto gap-10"
        @submit="
          (e) => {
            e.preventDefault()
            validate()

            if (stepIndex === steps.length && meta.valid) {
              onSubmit(values)
            }
          }
        "
      >
        <div class="flex flex-col gap-16">
          <StepperItem
            v-for="step in steps"
            :key="step.step"
            v-slot="{ state }"
            class="relative flex w-full items-start gap-6"
            :step="step.step"
          >
            <StepperSeparator
              v-if="step.step !== steps[steps.length - 1].step"
              class="absolute left-[18px] top-[38px] block h-[140%] w-0.5 shrink-0 rounded-full bg-muted group-data-[state=completed]:bg-primary"
            />

            <StepperTrigger as-child>
              <Button
                :variant="
                  state === 'completed' || state === 'active'
                    ? 'default'
                    : 'outline'
                "
                size="icon"
                class="z-10 rounded-full shrink-0"
                :class="[
                  state === 'active' &&
                    'ring-2 ring-ring ring-offset-2 ring-offset-background'
                ]"
                :disabled="state !== 'completed' && !meta.valid"
              >
                <Check
                  v-if="state === 'completed'"
                  class="size-5"
                />
                <Circle v-if="state === 'active'" />
                <Dot v-if="state === 'inactive'" />
              </Button>
            </StepperTrigger>

            <div class="flex flex-col gap-1">
              <StepperTitle
                :class="[state === 'active' && 'text-primary']"
                class="text-sm font-semibold transition lg:text-base"
              >
                {{ step.title }}
              </StepperTitle>
              <StepperDescription
                :class="[state === 'active' && 'text-primary']"
                class="sr-only text-xs text-muted-foreground transition md:not-sr-only lg:text-sm"
              >
                {{ step.description }}
              </StepperDescription>
            </div>
          </StepperItem>
        </div>

        <div class="flex flex-col gap-6 w-full">
          <div class="flex flex-col gap-4 mt-4">
            <template v-if="stepIndex === 1">
              <OrdersCreateStepOne />
            </template>

            <template v-if="stepIndex === 2">
              <OrdersCreateStepTwo />
            </template>

            <template v-if="stepIndex === 3">
              <OrdersCreateStepThree />
            </template>
          </div>

          <div class="flex items-center justify-between mt-4">
            <Button
              :disabled="isPrevDisabled"
              variant="outline"
              size="sm"
              @click="prevStep()"
            >
              Back
            </Button>
            <div class="flex items-center gap-3">
              <Button
                v-if="stepIndex !== 3"
                :type="meta.valid ? 'button' : 'submit'"
                :disabled="isNextDisabled"
                size="sm"
                @click="meta.valid && nextStep()"
              >
                Next
              </Button>
              <Button
                v-if="stepIndex === 3"
                size="sm"
                type="submit"
              >
                Submit
              </Button>
            </div>
          </div>
        </div>
      </form>
    </Stepper>
  </Form>
</template>
