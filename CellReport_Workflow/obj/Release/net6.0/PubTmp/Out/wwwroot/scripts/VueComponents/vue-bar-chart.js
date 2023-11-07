Vue.component('vue-bar-chart', {
    props: {
        barlist: { type: Array },
        i18n: { type: Boolean, default: false },
        rootUrl: { type: String, default: master.RootUrl }
    },
    computed: {
        reBarlist() {
            if (this.i18n) {
                this.barlist.forEach(function (element) {
                    element.text = $.i18n(element.text);
                });
                return this.barlist;
            } else {
                return this.barlist;
            }
        }
    },
    template: `
    <div>
    <div v-for="bar in reBarlist">
<div class="row">
        <div class="col-9 mb-0">
            <div class="progress progress-lg" 
                 style="margin-bottom: 1px;border-radius: 0;">
                <div v-if="bar.value>0"
                     class="progress-bar bg-success-600" 
                     style="text-align: right; padding: 5px 2px;position: inherit;"
                     v-bind:style="{width: bar.width}">
                    <span v-if="bar.widthValue>0.1">{{ bar.value }}</span>           
                </div>
                <span style="margin-left:2px;" v-if="bar.widthValue<=0.1">{{ bar.value }}</span>
            </div>
        </div>
        <div style="height:30px;" class="col-3 mb-0">
            <img v-if="bar.src"
                 v-bind:src="rootUrl + bar.src.substring(bar.src.indexOf('Media'),bar.src.length)"                 
                 style="width: 30px; height: 30px;">
            </img>
            {{ bar.text }}
        </div>
</div>
    </div>
    </div>`
});