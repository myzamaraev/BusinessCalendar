<template>
  
  <nav>
    <h3>BusinessCalendar</h3>
  <NavGroup name="">
    <NavItem :to="{name: 'about'}">About</NavItem>
  </NavGroup>
  <NavIdentifiersSection></NavIdentifiersSection>

  </nav> 
</template>

<script>
import NavItem from "../UI/NavItem.vue";
import NavGroup from "../UI/NavGroup.vue";
import NavIdentifiersSection from "./NavIdentifiersSection.vue";
export default {
  name: "the-navbar",
  components: {
    NavItem,
    NavGroup,
    NavIdentifiersSection
  },
}
</script>

<style scoped>
nav {
    color: #000!important;
    background-color: #f1f1f1!important;
}

@media screen and (min-width: 500px) {
  nav {
    width: 250px;
    height: 100%;
    position: fixed;
    max-width: 1000px;
    overflow: hidden;
    overflow-y: auto;
  }
}


</style>
