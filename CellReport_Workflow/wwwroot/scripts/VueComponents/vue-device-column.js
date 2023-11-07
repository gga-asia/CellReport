
Vue.component('vue-device-column', {
    props: {
        id: { type: String, required: false },
        icon: { type: String, required: false },
        name: { type: String, required: false },
        type: { type: String, default: 'text' },
        step: { type: String, default: '1' },
        accept: { type: String, default: 'image/*' },
        value: { type: [String, Number, Boolean], required: false },
        placeholder: { type: [String, Number], required: false },
        disabled: { type: Boolean, default: false },
        required: { type: Boolean, default: false },
        options: {
            type: Array, default: function () {
                return []
            }
        }
    }, data: function () {
        return {
            ImageVal: ""
        };
    }, mounted() {
        if (this.type === "img") {
            this.ImageVal = this.value;
        }

    },
    computed: {
        key() {
            return this.id || this.guid();
        },
        divId() {
            return this.id ? 'div' + this.id : this.guid();
        },


    },
    template: `
<div class="form-group" v-bind:class="{'mb-3':type!=='note'}">
	<label class="label title" :for="key" v-if="this.$slots.default">
		<slot></slot>
		<em v-if="required">*</em>
	</label>

	<div class="" v-if="type==='note'">

	</div>
	<div class="custom-control custom-checkbox" v-bind:id="divId" v-else-if="type==='checkbox'">
		<input class="custom-control-input" type="checkbox" v-bind:name="name" v-bind:id="key"
			v-bind:disabled="disabled" v-bind:checked='value' v-on:change="$emit('input', $event.target.checked)">
		<label class="custom-control-label" :for="key">&ensp;</label>
	</div>

	<div v-bind:id="divId" v-else-if="type==='img'">
		<input type="file" v-bind:accept="accept" class="upload-input" v-bind:id="'input_'+divId" style="display: none;" v-on:change="ReadInputFileImage($event)">
		<button type="button" class="btn mb-2 btn-default" style="padding: 5px 12px;" v-on:click="uploadImg"><i class="fas mr-1 fa-upload"></i> 上傳</button>
        <button type="button" class="btn mb-2 btn-default" style="padding: 5px 12px;" v-on:click="removeImg" v-if="ImageVal!=''"><i class="fas mr-1 fa-times"></i> 移除</button>
		<div v-if="ImageVal!=''"><img style="max-width: 100%;border:1px solid #E5E5E5;" v-bind:src="ImageVal" ></div>
	</div>

	<div class="input-group" v-bind:id="divId" v-else>
		<div class="input-group-prepend" v-show="icon">
			<span class="input-group-text">
				<i class="fas" v-bind:class="icon"></i>
			</span>
		</div>

		<input v-if="type==='number'" class="form-control" v-bind:type="type" v-bind:step="step" v-bind:name="name"
			v-bind:id="key" v-bind:placeholder="placeholder" v-bind:disabled="disabled" v-bind:value="value"
			@input="$emit('input', $event.target.value)" @keyup="$emit('keyup')" @change="$emit('change')" />


		<select @input="$emit('input', $event.target.value)" @keyup="$emit('keyup')" @change="$emit('change')"
			v-else-if="type==='select'" class="form-control" v-bind:type="type" v-bind:name="name" v-bind:id="key"
			v-bind:value="value">
			<option v-for="option in options" v-bind:value="option.value">{{option.text}}</option>

		</select>



		<input v-else class="form-control" v-bind:type="type" v-bind:name="name" v-bind:id="key"
			v-bind:placeholder="placeholder" v-bind:disabled="disabled" v-bind:value="value"
			@input="$emit('input', $event.target.value)" @keyup="$emit('keyup')" @change="$emit('change')" />

	</div>

</div>
      `,
    methods: {
        ReadInputFileImage(argObj) {
            var _this = this;
            var input = argObj.target;
            if (input.files && input.files[0]) {
                var reader = new window.FileReader();
                reader.onload = function () {
                   
                    _this.$emit("input", reader.result)
                    _this.ImageVal = reader.result;
                    //if (type == "A") {
                    //    member.ProfilePicture = reader.result.split(',')[1];
                    //} else {
                    //    member.RecognitionFace = reader.result.split(',')[1];
                    //}
                };
                reader.readAsDataURL(input.files[0]);
                input.value = '';
            }
        },
        uploadImg: function () {
            $('#input_' + this.divId).click();

        },
        removeImg() {
            this.$emit("input", '');
            this.ImageVal = '';
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