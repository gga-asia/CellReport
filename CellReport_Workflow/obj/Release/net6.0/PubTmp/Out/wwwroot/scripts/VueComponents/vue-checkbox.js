Vue.component('vue-checkbox', {
    props: {
        id: { type: String, required: false },
        name: { type: String, required: false },
        value: { type: Boolean, required: false },
        disabled: { type: Boolean, default: false },
        title: { type: String, required: false, default: "" },
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

  <div class="form-group">
        <div v-bind:id="divId"
            class="custom-control custom-checkbox custom-control-inline align-middle mx-auto">
            <input type="checkbox"
                class="custom-control-input"
                :name="name"
                :id="key"
                :disabled="disabled"
                v-bind:checked='value'
              
                 @change="changeEvent($event.target.checked)"
                >
            <label class="custom-control-label text-center"
                style="margin-left: 0.25rem"
                :for="key">
                <slot></slot>
                <i 
                    v-if="title!=''" 
                    style="cursor: pointer;"
                    class="fas fa-question-circle icon-tooltip"
                    data-placement="top"
                    :data-original-title="title"
                    ></i>

            </label>
        </div>
    </div>
      `,
    mounted() {
        var _this = this;
        if (_this.title != "") {
            _this.$nextTick(() => {
                $(_this.$el).find(".icon-tooltip").tooltip();
            })
        }
    },
    methods: {

        changeEvent(checked) {
            this.$emit('input', checked)
            this.$emit('change', checked)
        },

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