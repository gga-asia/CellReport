Vue.component('vue-checkbox-cell', {
    props: {
        id: { type: String, required: false },
        name: { type: String, required: false },
        value: { type: Boolean, required: false },
        disabled: { type: Boolean, default: false },
    },
    computed: {
        key() {
            return this.id || this.guid();
        },
        divId() {
            return this.id ? 'div' + this.id : this.guid();
        }

    },
    template: `
   <div>
        <div class="custom-control custom-checkbox custom-control-inline" :id="divId" :key="divId">
            <input type="checkbox" 
            class="custom-control-input" 
            :name="name" 
            :id="key" 
            :disabled="disabled" 
            v-bind:checked='value'
            v-on:change="$emit('input', $event.target.checked)">
            <label class="custom-control-label" 
                :for="key" 
                v-if="this.$slots.default">
                <slot></slot>
            </label>
        </div>
    </div>
      `,
    methods: {
        guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                    .toString(16)
                    .substring(1);
            }
            return (
                'input' +
                s4() +
                s4() +
                '-' +
                s4() +
                '-' +
                s4() +
                '-' +
                s4() +
                '-' +
                s4() +
                s4() +
                s4()
            );
        },
        focus() {
            this.$refs.input.focus();
        }
    }
});