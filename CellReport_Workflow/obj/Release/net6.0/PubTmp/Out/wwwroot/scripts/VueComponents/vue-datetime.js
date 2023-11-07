Vue.component('vue-datetime', {
    props: {
        id: { type: String, required: false },
        value: { type: [String, Number], required: false },
        icon: { type: String, required: false },
        name: { type: String, required: false },
        placeholder: { type: [String, Number], required: false },
        disabled: { type: Boolean, default: false },
        required: { type: Boolean, default: false },
        remind: { type: String, required: false }
    },
    data() {
        return { dateTime: '' };
    },
    mounted() {
        const _this = this;
        $('#' + _this.key).datetimepicker({
            step: 30,
            onClose() {
                const dateTime = $('#' + _this.key).val();
                if (
                    /^[0-9]{4}\/(0[1-9]|1[0-2])\/(0[1-9]|[1-2][0-9]|3[0-1]) (2[0-3]|[01][0-9]):[0-5][0-9]$/.test(
                        dateTime
                    )
                ) {
                    //to utc time
                    _this.dateTime = dateTime;
                    _this.$emit('input', dateTime); //for v-model
                    _this.$emit('change');
                    _this.$emit('utc', new Date(dateTime).toISOString());
                } else {
                    _this.dateTime = '';
                    _this.$emit('input', '');
                    _this.$emit('utc', '');
                }
            },
            i18n: {
                en: {
                    months: [
                        "1月",
                        "2月",
                        "3月",
                        "4月",
                        "5月",
                        "6月",
                        "7月",
                        "8月",
                        "9月",
                        "10月",
                        "11月",
                        "12月",
                    ],
                    dayOfWeekShort: [
                        "日",
                        "一",
                        "二",
                        "三",
                        "四",
                        "五",
                        "六",
                    ],
                    dayOfWeek: [
                        "星期日",
                        "星期一",
                        "星期二",
                        "星期三",
                        "星期四",
                        "星期五",
                        "星期六",
                    ]
                }
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
                'datetime' +
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
        :value="value?value:dateTime"
        :icon="icon"
        :name="name"
        :placeholder="placeholder"
        :disabled="disabled"
        :required="required"
        :remind="remind"
      >
        <slot></slot>
      </vue-input>
  
    `
});