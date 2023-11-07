Vue.component('vue-select', {
    props: {
        id: { type: String, required: false },
        required: { type: Boolean, default: false },
        disabled: { type: Boolean, default: false },
        icon: { type: String, required: false },
        name: { type: String, required: false },
        value: { type: [String, Number], required: false },
        options: {
            type: Array,
            default: () => []
        },
        placeholder: {
            type: Object,
            required: false
        },
        i18n: { type: Boolean, default: false },
        label: { type: String, default: 'text' },
        trackby: { type: String, default: 'value' },
        disabledby: { type: String, default: 'disabled' },
        disabledtitle: { type: Boolean, default: false },
        title: { type: String, required: false, default: "" },
    },
    computed: {
        key() {
            return this.id ? this.id : this.guid();
        },
        reOptions() {
            let _this = this;
            if (this.i18n) {
                this.options.forEach(function (element) {
                    element[_this.label] = $.i18n(element[_this.label]);
                });
                return this.options;
            } else {
                return this.options;
            }
        }
    },
    template: `
<div class="form-group">
    <label class="form-label" v-if="!disabledtitle">
        <slot></slot>
        <em v-if="required">*</em>
        <i 
        v-if="title!=''" 
        style="cursor: pointer;"
        class="fas fa-question-circle icon-tooltip"
        data-placement="top"
        :data-original-title="title"
        ></i>
    </label>
    <div class="input-group">
        <div class="input-group-prepend" v-show="icon">
            <span class="input-group-text">
                <i class="fas" :class="icon"></i>
            </span>
        </div>
        <select class="form-control" 
                @change="$emit('change')" 
                :name="name" 
                :id="key" 
                :disabled="disabled" 
                :value="value" 
                @input="$emit('input', $event.target.value)">
            <option
              v-if="placeholder"
              :value="placeholder.value">
              {{placeholder.text}}
            </option>

            <option 
                v-for="(option,index) in reOptions" 
                :value="option[trackby]" 
                :disabled="option[disabledby]"
                :selected="option[trackby] == value">
                {{ option[label] }}
            </option>
        </select>
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
        guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                    .toString(16)
                    .substring(1);
            }
            return (
                'select' +
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
    }
});