Vue.component('vue-datepicker', {
    props: {
        id: { type: String, required: false },
        value: { type: [String, Number], required: false },
        icon: { type: String, required: false },
        name: { type: String, required: false },
        placeholder: { type: [String, Number], required: false },
        disabled: { type: Boolean, default: false },
        required: { type: Boolean, default: false },
        mindate: { type: String, required: false, default:""},
        maxdate: { type: String, required: false, default: "" },
        format: { type: String, required: false, default: "Y/m/d" }
    },
    data() {
        return { date: '' };
    },
    mounted() {
        const _this = this;
        $('#' + _this.key).datetimepicker({
            scrollInput: false,
            timepicker: false,
            format: _this.format,
            onClose() {
                const date = $('#' + _this.key).val();
                _this.date = date;
                _this.$emit('input', date);
            },
        });
        if (_this.mindate != "") {
            $('#' + _this.key).datetimepicker({ minDate: _this.mindate})
        }
        if (_this.maxdate != "") {
            $('#' + _this.key).datetimepicker({ maxDate: _this.maxdate })
        }

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
                'datepicker' +
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