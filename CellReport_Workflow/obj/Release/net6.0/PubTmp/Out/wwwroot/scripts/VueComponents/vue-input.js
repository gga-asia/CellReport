Vue.component('vue-input', {
    props: {
        id: { type: String, required: false },
        icon: { type: String, required: false },
        name: { type: String, required: false },
        type: { type: String, default: 'text' },
        value: { type: [String, Number], required: false },
        placeholder: { type: [String, Number], required: false },
        disabled: { type: Boolean, default: false },
        required: { type: Boolean, default: false },
        remind: { type: String, required: false },
        title: { type: String, required: false, default: "" },
    },
    computed: {
        key() {
            return this.id || this.guid();
        },
        divId() {
            return this.id ? 'div' + this.id : this.guid();
        },
        reType() {
            return this.type;
        }
       
    },
    template: `

<div class="form-group mb-2">
    <label 
        class="label title" 
        :for="key" 
        >
        <slot></slot>
       
        <i v-if="!!remind" class="text-muted" v-text="'(' + remind + ')'"></i>
          <i 
        v-if="title!=''" 
        style="cursor: pointer;"
        class="fas fa-question-circle icon-tooltip"
        data-placement="top"
        :data-original-title="title"
        ></i>
         <em v-if="required">*</em>
    </label>
    <div class="input-group" :id="divId">
        <div class="input-group-prepend" v-show="icon">
            <span class="input-group-text">
                <i class="fas" :class="icon"></i>
            </span>
        </div>
        <input
              class="form-control"
              :type="reType"
              :name="name"
              :id="key"
              :placeholder="placeholder"
              :disabled="disabled"
              :value="value"
              ref="input"
              @input="$emit('input', $event.target.value)"
              @keyup="$emit('keyup')"
              @change="$emit('change')"
              @click="$emit('click')"
            />
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