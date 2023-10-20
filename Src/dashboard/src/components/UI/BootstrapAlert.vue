<template>
   <div :class="['alert', alertType ]">
        <slot>
            <span v-if="!text">This is a {{type}} alert</span>
        </slot>
        <slot name="link">
            <a v-if="link" :href="link" class="alert-link">
                <span v-if="!linkText">{{ link }}</span>
            </a>
        </slot>
    </div>
</template>

<script>
export default {
    name: "b-alert",
    props: {
        type: {
            type: String,
            required: false,
            default: "primary"
        },
        text: String,
        link: String,
        linkText: String
    },
    computed: {
        alertType() {
            return 'alert-'+this.type;
        }
    }
}
</script>

<style scoped>

</style>