
<script setup lang="ts">
const {handleVisibilityChange, stopSilentRefresh} = useAuth()

onMounted(() => {
    // Add event listener for visibility change
    document.addEventListener('visibilitychange', handleVisibilityChange);

    // Initially start the silent refresh if the tab is already active
    // if (document.visibilityState === 'visible') {
    //   startSilentRefresh();
    // }
  });

  onBeforeUnmount(() => {
    // Clean up event listener and interval when component unmounts
    document.removeEventListener('visibilitychange', handleVisibilityChange);
    stopSilentRefresh();
  });
</script>
<template>
  <div>
    <NuxtLoadingIndicator color="#0891B2" :height="5" :throttle="0"/>
    <NuxtLayout>
      <NuxtPage />
    </NuxtLayout>
  </div>
</template>
