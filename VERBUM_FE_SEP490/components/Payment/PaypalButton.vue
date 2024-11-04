<script lang="ts" setup>
import { onMounted, ref } from 'vue';
import { loadScript } from '@paypal/paypal-js';
import type { PayPalScriptOptions, OnApproveActions, OnApproveData, PayPalNamespace } from '@paypal/paypal-js';

const paypalButton = ref<HTMLDivElement | null>(null);

onMounted(async () => {
  if (paypalButton.value) {
    const options: PayPalScriptOptions = {
      "clientId": "AfA7cxuOdrPU6GvcxtuYqCNYrO_k2EcgtZhsI5Kl3Z_0r3_0lIHy1JymnEYuR31tGFqr2-lEIulHsTRL",  // Replace with your actual client ID
      currency: "USD"
    };

    const paypal: PayPalNamespace | null = await loadScript(options);

    if (paypal && paypal.Buttons) {
      paypal.Buttons({
        createOrder: (data, actions) => {
          return actions.order.create({
            intent: "CAPTURE",  // Specify the intent for the order
            purchase_units: [{
              amount: {
                currency_code: "USD",
                value: "10.00"  // Set dynamically if needed
              }
            }]
          });
        },
        onApprove: (data: OnApproveData, actions: OnApproveActions) => {
          if (!actions.order) {
            console.error('Order is undefined');
            return Promise.reject('Order is undefined');
          }
          
          return actions.order.capture().then((details) => {
            if (details?.payer?.name?.given_name) {
              alert(`Transaction completed by ${details.payer.name.given_name}`);
            } else {
              alert("Transaction completed.");
            }
          });
        },
        onError: (err) => {
          console.error('PayPal Error:', err);
        }
      }).render(paypalButton.value);
    } else {
      console.error("PayPal script failed to load.");
    }
  }
});
</script>

<template>
  <div ref="paypalButton"/>
</template>

<style scoped>
/* Add any custom styling if needed */
</style>
