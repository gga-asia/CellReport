Vue.component('vue-timepicker', {
    props: {
        id: { type: String, required: false },
        value: { type: [String, Number], required: false },
        icon: { type: String, required: false },
        name: { type: String, required: false },
        placeholder: { type: [String, Number], required: false },
        disabled: { type: Boolean, default: false },
        required: { type: Boolean, default: false }
    },
    data() {
        return { time: '' };
    },
    mounted() {
        const _this = this;
        $('#' + _this.key).datetimepicker({
            datepicker: false,
            format: 'H:i',
            onClose() {
                const time = $('#' + _this.key).val();
                _this.time = time;
                _this.$emit('input', time);
            }
        });
    },
    computed: {
        key() {
            return this.id ? this.id : this.guid();
        }
    },
    methods: {
        guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                    .toString(16)
                    .substring(1);
            }
            return (
                'timepicker' +
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
        }
    },
    template: `
      <vue-input
        :id="key"
        :value="value"
        :icon="icon"
        :name="name"
        :placeholder="placeholder"
        :disabled="disabled"
        :required="required"
        @keyup="$emit('keyup')"
      >
        <slot></slot>
      </vue-input>
    `
});