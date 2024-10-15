<script>
export default {
  crumbs() {
    const fullPath = this.$route.fullPath
    const params = fullPath.substring(1).split('/')
    const crumbs = []

    console.log(params)

    // url:     /blog/2020/11/20/my-post-url
    // outputs: ['blog','2020','11','20','my-post-url']
    return crumbs
  }
}
</script>

<template>
  <Breadcrumb>
    <BreadcrumbList>
      <BreadcrumbItem>
        <BreadcrumbLink>
          <a href="/"> Home </a>
        </BreadcrumbLink>
      </BreadcrumbItem>
      <BreadcrumbSeparator />
      <BreadcrumbItem>
        <BreadcrumbLink>
          <a href="/docs/components/accordion.html"> Components </a>
        </BreadcrumbLink>
      </BreadcrumbItem>
      <BreadcrumbSeparator />
      <BreadcrumbItem>
        <BreadcrumbPage>Breadcrumb</BreadcrumbPage>
      </BreadcrumbItem>
    </BreadcrumbList>
  </Breadcrumb>
</template>
