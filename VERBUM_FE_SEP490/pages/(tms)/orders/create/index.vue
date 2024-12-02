<script setup lang="ts">
import { Check, Circle, Dot } from 'lucide-vue-next'
import { toTypedSchema } from '@vee-validate/zod'
import * as z from 'zod'
import { ref } from 'vue'
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
import { useAPI } from '@/composables/useCustomFetch'
import { useToast } from '~/components/ui/toast'

const { toast } = useToast()
const router = useRouter()

useSeoMeta({
  title: 'Create Order'
})

const formSchema = [
  z.object({
    sourceLanguageId: z.string(),
    targetLanguageIdList: z.string(),
    translationFileURL: z.string().min(1, { message: 'Required' }),
    dueDate: z.coerce.date()
  }),
  z.object({
    hasTranslateService: z.boolean().default(false),
    hasEditService: z.boolean().default(false),
    hasEvaluateService: z.boolean().default(false)
  }),
  z.object({
    reference: z.string().optional(),
    referenceFileURLs: z.string().optional(),
    discountId: z.string().nullable().optional()
  })
]

interface FormValues {
  sourceLanguageId: string
  targetLanguageIdList: string
  translationFileURL: string
  dueDate: Date
  hasTranslateService?: boolean
  hasEditService?: boolean
  hasEvaluateService?: boolean
  reference?: string
  referenceFileURLs: string
  discountId?: string
}

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

async function onSubmit(values: FormValues) {
  const payload = {
    ...values,
    sourceLanguageId: values.sourceLanguageId,
    targetLanguageIdList: values.targetLanguageIdList
      .split(',')
      .map((id: string) => id.trim()),
    translationFileURL: values.translationFileURL
      .split(',')
      .map((id: string) => id.trim()),
    dueDate: format(values.dueDate, "yyyy-MM-dd'T'HH:mm:ss"),
    hasTranslateService: values.hasTranslateService ?? false,
    hasEditService: values.hasEditService ?? false,
    hasEvaluateService: values.hasEvaluateService ?? false,
    reference: values.reference,
    referenceFileURLs: values.referenceFileURLs
      .split(',')
      .map((url: string) => url.trim()),
    discountId: values.discountId
  }

  try {
    const { status } = await useAPI('/order/add', {
      method: 'POST',
      body: JSON.stringify(payload),
      headers: {
        'Content-Type': 'application/json'
      }
    })
    if (status.value === "success") {
      toast({
        title: 'Order Created !!',
        description: `Order has been created successfully!!`
      })
      const response = await repo(useNuxtApp().$api).getOrders()
      if (!response){
        toast({
          title: 'Error Fetching Orders',
          description: 'An error occurred while fetching the Orders!!'
        })
        return
      }
      const createdOrder = response[0]
      router.push('/orders/details/' + createdOrder.orderId)
    }
  } catch (err) {
    console.error('Request failed:', err)
    toast({
      title: 'Error create Order',
      description: 'An error occurred while creating the Order!!'
    })
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
                <Check v-if="state === 'completed'" class="size-5" />
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
              <Button v-if="stepIndex === 3" size="sm" type="submit">
                Submit
              </Button>
            </div>
          </div>
        </div>
      </form>
    </Stepper>
  </Form>
</template>
