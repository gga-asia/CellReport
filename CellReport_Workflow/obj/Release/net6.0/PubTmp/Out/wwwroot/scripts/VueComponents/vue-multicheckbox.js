Vue.component('vue-multicheckbox', {
    model: {
        prop: 'value',
        event: 'change'
    },
    props: {
        required: { type: Boolean, required: false, default: false },
        id: { type: String, required: false },
        name: { type: String, required: false },
        options: { type: Array, required: false, default: [] },
        checked: { type: Array, required: false, default: [] }
        //[{value:'one',text:'One',disabled:true,selected:true}...]
    },
    computed: {
        divId() {
            return 'div' + (this.id || guid('multicheckbox'));
        },
        key() {
            return this.id || guid('multicheckbox');
        },
        text() {
            if (!this.$slots.default) return '';
            var slot = this.$slots.default[0].text;
            return $.i18n(slot.trim());
        }
    },
    data() {
        return { check_array: [], i18n: $.i18n };
    },
    watch: {
        checked: function (array) {
            this.check_array = array;
        }
    },
    mounted() {
        this.check_array = this.checked;
    },
    methods: {},
    template: `
<div class="frame-wrap" :id="key">
        <div class="custom-control custom-checkbox" v-for="choice in options">
            <input type="checkbox" class="custom-control-input" :id="key+'-'+choice.value" v-model="check_array" :value="choice" @change="$emit('change_array', check_array)">
            <label class="custom-control-label" :for="key+'-'+choice.value">{{i18n(choice.text)}}</label>
        </div>
</div>
      `
});