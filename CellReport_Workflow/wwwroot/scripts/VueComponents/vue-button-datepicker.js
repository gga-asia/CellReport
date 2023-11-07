Vue.component('vue-button-datepicker', {
    props: {
        id: {
            type: String,
            required: false
        },
    },
    data() {
        return { date: '' };
    },
    mounted() {
        const _this = this;
        $('#' + _this.key).datetimepicker({
            timepicker: false,
            format: 'Y/m/d',
           
            onSelectDate() {
                 const date = $('#' + _this.key).val();
                _this.date = date;
                _this.$emit('change', date);
            }, i18n: {
                en: {
                    months: [
                        $.i18n('January'),
                        $.i18n('February'),
                        $.i18n('March'),
                        $.i18n('April'),
                        $.i18n('May'),
                        $.i18n('June'),
                        $.i18n('July'),
                        $.i18n('August'),
                        $.i18n('September'),
                        $.i18n('October'),
                        $.i18n('November'),
                        $.i18n('December')
                    ],
                    dayOfWeekShort: [
                        $.i18n('Sun'),
                        $.i18n('Mon'),
                        $.i18n('Tue'),
                        $.i18n('Wed'),
                        $.i18n('Thu'),
                        $.i18n('Fri'),
                        $.i18n('Sat')
                    ],
                    dayOfWeek: [
                        $.i18n('Sunday'),
                        $.i18n('Monday'),
                        $.i18n('Tuesday'),
                        $.i18n('Wednesday'),
                        $.i18n('Thursday'),
                        $.i18n('Friday'),
                        $.i18n('Saturday')
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
        <button :id="key"
           class="btn waves-effect waves-themed" type="button" style="background: #bff5b5 !important;border-color: rgb(204, 204, 204);">
            <i class="fas fa-calendar-alt"></i>
             <slot></slot>
        </button>
    `
});