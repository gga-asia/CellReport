Vue.component('vue-button', {
    props: {
        icon: {
            type: String,
            required: false
        },
        color: {
            type: String,
            default: 'primary',
            validator: function (value) {
                return (
                    [
                        'primary',
                        'success',
                        'info',
                        'warning',
                        'danger',
                        'secondary',
                        'light',
                        'default'
                    ].indexOf(value) !== -1
                );
            }
        },
        loading: {
            type: Boolean,
            default: () => false
        },
        disabled: {
            type: Boolean,
            default: () => false
        }
    },
    computed: {
        colorClass() {
            return 'btn-' + this.color;
        },
        faicon() {
            if (this.loading) {
                return 'fa-refresh fa-spin';
            } else {
                return this.icon;
            }
        },
        isDisabled() {
            if (this.disabled || this.loading) {
                return true;
            } else {
                return false;
            }
        }
    },
    template: `

    <button
        @click="$emit('click')"
        type="button" 
        class="btn"
        :class="[colorClass,icon?'btn-labeled':'']" 
        :disabled="isDisabled">
        <i class="fas mr-1" :class="faicon" v-show="icon"></i>
    <slot></slot>
    </button>

    `
});