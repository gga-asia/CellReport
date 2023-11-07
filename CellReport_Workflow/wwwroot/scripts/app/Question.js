$(document).ready(() => {

    Vue.directive('autoresize', {
        inserted: function (el) {
            el.addEventListener('input', function () {
                this.style.height = '120px';
                if (this.scrollHeight > 120) {
                    this.style.height = this.scrollHeight + 'px';
                }
            });
        }
    });

    var content = new Vue({
        el: "#content",
        data: {
            ReadOnly: master.API.ReadOnly,
            fromString: "",
            RootUrl: master.RootUrl,
            IsPreview: master.API.IsPreview,
            Page: 1,
            IsLoading: true,
            SurveyId: "",
            SurveyData: {
                Title: "",
                PrefaceMessage: ""
            },
            QuestionDatas: [],
            SurveyProgress: 0,
            TotalQuestions: 0,
            IsReadQuestions: 0,
            FileAcceptInfo: {
                Word: [".doc", ".docx", ".pdf"],
                Image: [".png", ".jpeg", ".jpg"],
                PPT: [".ppt", ".pptx"],
                Video: [".mp4"],
                Excel: [".xlsx", ".xls"],
                Aidio: [".mp3"]
            }
            , LinkQuestions: []
            , Answer: {}
        }, components: {
            loading: VueLoading

        },
        mounted() {
            var urlParams = new URLSearchParams(window.location.search);
            this.SurveyId = urlParams.get('Id');
            this.fromString = urlParams.get('fromString') ?? "";
            document.title = "問卷"
            this.LoadSurvey();

        }, methods: {
            RemoveFile(item) {
                item.Answer = "";
                item.OtherAnswer = "";
            },
            GetAcceptList(item) {
                var accetps = [];

                if (item.UploadSetting.FileFormatRestrictions) {
                    accetps.push("*");
                    if (item.UploadSetting.Word) {
                        accetps = accetps.concat(this.FileAcceptInfo["Word"])
                    }
                    if (item.UploadSetting.Image) {
                        accetps = accetps.concat(this.FileAcceptInfo["Image"])
                    }
                    if (item.UploadSetting.PPT) {
                        accetps = accetps.concat(this.FileAcceptInfo["PPT"])
                    }
                    if (item.UploadSetting.Video) {
                        accetps = accetps.concat(this.FileAcceptInfo["Video"])
                    }
                    if (item.UploadSetting.Excel) {
                        accetps = accetps.concat(this.FileAcceptInfo["Excel"])
                    }
                    if (item.UploadSetting.Aidio) {
                        accetps = accetps.concat(this.FileAcceptInfo["Aidio"])
                    }
                } else {
                    accetps.push("*");
                }
                return accetps;
            },
            GetFileAcceptStr(item) {

                return this.GetAcceptList(item).join(',');
            },
            BeautySub(str, len) {
                var reg = /[\u4e00-\u9fa5]/g,    //专业匹配中文
                    slice = str.substring(0, len),
                    chineseCharNum = (~~(slice.match(reg) && slice.match(reg).length)),
                    realen = slice.length * 2 - chineseCharNum;
                return str.substr(0, realen) + (realen < str.length ? "..." : "");
            },
            HandleFileUpload(item, event) {

                var file = event.target.files[0];
                item.IsMaxFileSizeAlert = false;
                item.IsFileFormatAlert = false;
                item.Answer = "";
                item.OtherAnswer = "";
                if (!!file) {
                    var maxFileSizeInBytes = item.UploadSetting.FileSize * 1024 * 1024;
                    if (file.size > maxFileSizeInBytes) {
                        item.IsMaxFileSizeAlert = true;
                        event.target.value = ''
                        this.IsReadAction(item);
                        return;
                    }
                    if (item.UploadSetting.FileFormatRestrictions) {
                        var accept = this.GetAcceptList(item);
                        var fileExtension = "." + file.name.match(/\.[0-9a-z]+$/i)[0].substring(1);
                        if (accept.findIndex(e => e == fileExtension) == -1) {
                            item.IsFileFormatAlert = true;
                            event.target.value = ''
                            this.IsReadAction(item);
                            return;
                        }
                    }

                    var reader = new FileReader();
                    item.Answer = file.name;
                    reader.onload = function () {


                        item.OtherAnswer = reader.result;
                    }
                    reader.readAsDataURL(file);

                } else {

                }
                this.IsReadAction(item);
                event.target.value = ''; // 清空input，讓使用者重新選擇檔案

            },
            TriggerFileUploadClick(item) {

                //this.$refs['filebtn_' + item.Id][0].disabled = true;
                this.$refs['file_' + item.Id][0].click();

            },
            CheckRequestNeedAlert(item, isRead) {
                item.IsShowRequestNeedAlert = false;
                if (item.RequestNeed && (item.IsRead || isRead) && item.IsShow) {
                    if (item.Type == 1 || item.Type == 2 || item.Type == 5 || item.Type == 6) {
                        if (item.Answer.trim() == "") {
                            item.IsShowRequestNeedAlert = true;
                        }
                    } else if (item.Type == 3 || item.Type == 4) {

                        if (item.AnswerDatas.length == 0) {
                            item.IsShowRequestNeedAlert = true;
                        }
                    }

                }
                return item.IsShowRequestNeedAlert;
            },
            SurveyProgressPercent() {
                return Math.round(this.SurveyProgress * 100) + "%";
            },
            NumberToLetter(number) {
                const letters = 'abcdefghijklmnopqrstuvwxyz';

                if (number <= 26) {
                    return letters[number - 1];
                } else {
                    let result = '';

                    while (number > 0) {
                        const remainder = (number - 1) % 26;
                        result = letters[remainder] + result;
                        number = Math.floor((number - 1) / 26);
                    }

                    return result;
                }
            },
            IsReadAction(item) {
                var _this = this;
                item.IsRead = true;

                this.CheckRequestNeedAlert(item, false);
                //計算左下角進度條
                _this.TotalQuestions = 0;
                _this.IsReadQuestions = 0;

                _this.QuestionDatas.forEach((item) => {
                    if (!(item.Type == 7 || item.Type == 8) && item.IsShow) {
                        _this.TotalQuestions++
                    }
                    if (!(item.Type == 7 || item.Type == 8) && item.IsShow && item.IsRead) {
                        _this.IsReadQuestions++
                    }
                    _this.SurveyProgress = Math.round((this.IsReadQuestions / this.TotalQuestions) * 100) / 100;
                })


            },
            AnswerAction(item, answerId, action) {
                if (this.ReadOnly) {
                    return;
                }


                var type = item.Type;

                if (type == 3) {
                    item.AnswerDatas = [];
                    item.TopicOptions.forEach((data) => {
                        data.IsActive = false;
                        if (data.Id == answerId && !action) {
                            data.IsActive = true;
                            var data = [{
                                AnswerId: answerId,

                            }];
                            item.AnswerDatas = data;
                        }


                    })



                }
                else if (type == 4) {

                    if (answerId == "-2" && !action) {
                        item.TopicOptions.forEach((data) => {
                            if (data.Id != answerId) {
                                data.IsActive = false;
                                data.Other = "";
                            } else {
                                data.IsActive = !action;
                            }
                        })
                    }
                    else {
                        item.TopicOptions.forEach((data) => {
                            if (data.Id == "-2") {
                                data.IsActive = false;
                            }
                            else if (data.Id == answerId) {
                                data.IsActive = !action;
                                if (answerId == "-1" && !data.IsActive) {
                                    data.Other = "";
                                }
                            }
                        })
                    }


                    item.AnswerDatas = [];
                    item.TopicOptions.forEach((data) => {
                        if (data.IsActive) {
                            item.AnswerDatas.push({
                                AnswerId: data.answerId,
                            })
                        }
                    });


                }

                this.QuestionIsShow(item);


                this.IsReadAction(item);

            }, QuestionIsShow(item) {
                var _this = this;
                item.TopicOptions.forEach(option => {

                    //找到該選項對應的題目
                    var qlinks = _this.LinkQuestions.filter(link => link.FromId == item.Id && link.FromOptionId == option.Id);
                    qlinks.forEach(qlink => {
                        //找到該題目
                        var q = _this.QuestionDatas.find(q => q.Id == qlink.ToId);
                        if (!!q) {
                            //如果選項有被點選
                            if (option.IsActive) {
                                //該題目顯示
                                q.IsShow = true;
                            }
                            //如果選項沒被點選
                            else {
                                //找到該題目被綁定的選項，反推其他選項有沒有被點選
                                var optionlinks = _this.LinkQuestions.filter(link => q.Id == link.ToId);
                                var yes = false;
                                optionlinks.forEach(optionlink => {
                                    //找到該選項題目
                                    var qq = _this.QuestionDatas.find(q => q.Id == optionlink.FromId);
                                    if (!!qq) {
                                        if (qq.TopicOptions.find(q => q.Id == optionlink.FromOptionId && q.IsActive)) {
                                            yes = true;
                                            return;
                                        }
                                    }

                                });
                                if (!yes) {
                                    //不顯示
                                    q.IsShow = false;
                                    //清空資料
                                    q.Answer = "";
                                    q.OtherAnswer = "";
                                    q.TopicOptions.forEach(option => {
                                        option.IsActive = false;
                                        option.Other = "";
                                    });
                                    _this.QuestionIsShow(q);
                                    //_this.IsReadAction(q);
                                }

                            }

                        }
                    })

                })



                //var data = _this.LinkQuestions.filter



            },
            Guid() {
                function s4() {
                    return Math.floor((1 + Math.random()) * 0x10000)
                        .toString(16)
                        .substring(1);
                }
                return (

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
            LoadSurvey() {
                var _this = this;
                if (_this.ReadOnly) {

                    _this.ReturnAjax(
                        master.API.GetSurvey,
                        {
                            "SurveyId": _this.SurveyId,
                            "FromString": _this.fromString
                        },
                        (res) => {
                            if (res.IsSuccess) {
                                _this.SurveyData = res.Data.SurveyData;
                                document.title = _this.SurveyData.Title;
                                _this.ProcessQuestionDatas();
                                if (res.Data.Answers.length == 1) {
                                    _this.Answer = res.Data.Answers[0];
                                    _this.ProcessAnswerData();
                                } else {
                                    _this.Page=6
                                }
                              


                            } else {
                                _this.Page = 3;
                            }



                        }, "IsLoading"
                    )




                } else {
                    _this.ReturnAjax(
                        master.API.GetSurvey,
                        { "SurveyId": _this.SurveyId },
                        (res) => {
                            if (res.IsSuccess) {
                                if (res.Data.Id != null) {
                                    _this.SurveyData = res.Data;


                                    if (_this.IsPreview) {
                                        document.title = _this.SurveyData.Title;
                                        _this.ProcessQuestionDatas();
                                    } else {
                                        if (_this.SurveyData.Status == 0) {
                                            _this.Page = 4;
                                            document.title = "本問卷尚未公開！"
                                            return;
                                        } else if (_this.SurveyData.ValidTimeActive == 1) {

                                            var now = moment();
                                            var start = moment(_this.SurveyData.ValidStartTime);
                                            var end = moment(_this.SurveyData.ValidEndTime)
                                            if (!(start.isBefore(now) && now.isBefore(end))) {
                                                _this.Page = 5;
                                                document.title = "本問卷不在有效期間！"
                                                return;
                                            }

                                        }


                                        document.title = _this.SurveyData.Title;
                                        _this.ProcessQuestionDatas();

                                    }

                                } else {
                                    _this.Page = 3;
                                }



                            } else {
                                _this.Page = 3;
                            }



                        }, "IsLoading"
                    )
                }





            },
            //加工答案
            ProcessAnswerData() {
                var _this = this;
                _this.QuestionDatas.forEach(question => {     
                    if (question.Type == 1 || question.Type == 2 || question.Type == 5 || question.Type == 9) {
                        var answer = _this.Answer.List.find(a => a.Id == question.Id);
                        if (!!answer) {
                            answer.Answers.forEach(a => question.Answer = a.Answer);
                        }
                    } else if (question.Type == 6) {
                        var answer = _this.Answer.List.find(a => a.Id == question.Id);
                        if (!!answer) {
                            answer.Answers.forEach(a => question.Answer = a.Answer);
                            if (question.Answer != "") {
                                question.OtherAnswer = master.RootUrl + "Api/v/File.ashx?";
                                question.OtherAnswer += "s=" + this.SurveyId;
                                question.OtherAnswer += "&fromString=" + _this.fromString;
                                question.OtherAnswer += "&q=" + question.Id;
                                question.OtherAnswer += "&e=" + question.Answer.match(/\.[0-9a-z]+$/i)[0].substring(1);
                            }
                           
                           
                        }
                    }
                    else if (question.Type == 3 || question.Type == 4) {
                        var answer = _this.Answer.List.find(a => a.Id == question.Id);
                        if (!!answer) {
                            question.TopicOptions.forEach(to => {         
                                answer.Answers.forEach(a => {
                                    if (a.Answer == to.Id) {
                                        to.IsActive = true;
                                        to.Other = a.Other;
                                    }
                                });
                            })
                        }
                        _this.QuestionIsShow(question);

                    }
                })

            },
            //加工題目
            ProcessQuestionDatas() {
                var _this = this;
                var number1 = 1;


                var questionDatas = [];
                //處理題目
                _this.SurveyData.QuestionDatas.forEach((questionData, index) => {
                    var item = {};
                    item.Id = questionData.Id;
                    item.Index = questionData.Index;
                    item.Remark = questionData.Remark;
                    item.SurveyId = questionData.SurveyId;
                    item.Title = questionData.Title;
                    item.Type = questionData.Type;
                    item.IsShowRequestNeedAlert = false;
                    item.IsMaxFileSizeAlert = false;
                    item.IsFileFormatAlert = false;
                    item.IsRead = false;
                    //item.IsShow = true;

                    var setting = JSON.parse(questionData.JsonSetting);

                    //題號處理
                    item.IndentNeed = setting.IndentNeed;
                    item.RequestNeed = setting.RequestNeed;
                    item.RemarkNeed = setting.RemarkNeed;
                    item.UploadSetting = setting.UploadSetting;

                    if (typeof setting.Other === "undefined") {
                        setting.Other = {
                            Need: false,
                            Title: "",
                        }
                    }
                    if (typeof setting.None === "undefined") {
                        setting.None = {
                            Need: false,
                            Title: "",
                        }
                    }
                    item.Other = setting.Other;
                    item.None = setting.None;

                    item.Number = " ";
                    //有縮排
                    if (item.IndentNeed) {
                        var preItem = _this.QuestionDatas[index - 1];
                        if (preItem === undefined || !preItem.IndentNeed) {
                            item.Number = 1;
                        }
                        else if (preItem.IndentNeed) {
                            item.Number = preItem.Number + 1;
                        }
                    }
                    else if (item.Type == 7 || item.Type == 8) {
                        //data.Number = " ";

                    }
                    //沒縮排
                    else {
                        item.Number = number1;
                        number1++;
                    }
                    //

                    //題目選項
                    item.TopicOptions = [];
                    //單選題
                    if (item.Type == 3) {
                        setting.TopicOptions.forEach((data, index) => {

                            var topicOption = {
                                IsOther: false,
                                Title: data.Title,
                                IsActive: false,
                                Other: "",
                                Id: data.Id
                            }
                            item.TopicOptions.push(topicOption);
                        });
                        if (item.Other.Need) {
                            item.TopicOptions.push({
                                IsOther: true,
                                Title: item.Other.Title,
                                IsActive: false,
                                Other: "",
                                Id: "-1"
                            });
                        }

                    }
                    //多選題
                    else if (item.Type == 4) {
                        setting.TopicOptions.forEach((data, index) => {

                            var topicOption = {
                                IsOther: false,
                                Title: data.Title,
                                IsActive: false,
                                Other: "",
                                Id: data.Id
                            }
                            item.TopicOptions.push(topicOption);
                        });
                        if (item.Other.Need) {
                            item.TopicOptions.push({
                                IsOther: true,
                                Title: item.Other.Title,
                                IsActive: false,
                                Other: "",
                                Id: "-1"
                            });
                        }
                        if (item.None.Need) {
                            item.TopicOptions.push({
                                IsNone: true,
                                Title: item.None.Title,
                                IsActive: false,
                                Other: "",
                                Id: "-2"
                            });
                        }

                    }
                    //計算總題目數量
                    //if (!(item.Type == 7 || item.Type == 8)) {
                    //    _this.TotalQuestions++
                    //}

                    item.IsShow = true;

                    //客人回答
                    item.Answer = "";
                    item.OtherAnswer = "";
                    item.AnswerDatas = [];
                    _this.QuestionDatas.push(item);

                });

                //處理接題
                _this.SurveyData.LinkQuestions.forEach((item) => {
                    item.LinkQuestions.forEach((link) => {
                        var data = {
                            FromId: item.Id,
                            FromOptionId: item.OptionId,
                            ToId: link,
                            Type: 0,
                        }
                        _this.LinkQuestions.push(data);
                    })
                });





                //初始化顯示
                _this.QuestionDatas.forEach((item) => {
                    if (_this.LinkQuestions.filter(e => e.ToId == item.Id).length > 0) {
                        item.IsShow = false
                    }
                    if (!(item.Type == 7 || item.Type == 8) && item.IsShow) {
                        _this.TotalQuestions++
                    }
                })


            },


            ReturnAjax(url, data, callback, loadingName) {
                var _this = this;
                //if (loadingName == undefined) loadingName = 'IsLoading';
                //if (loadingName == undefined) loadingName = 'IsLoading';
                return $.ajax({
                    url: master.RootUrl + url,
                    type: "POST",
                    async: true,
                    cache: false,
                    data: data,
                    dataType: 'json',
                    //之前
                    beforeSend() {
                        if (loadingName != undefined) {
                            _this[loadingName] = true;
                        }

                    },
                    //成功
                    success: function (result) {
                        if (result.StatusCode == 200) {
                            callback(result);
                        } else {
                            //openWarningDialog(result.StatusCode + ' : ' + result.StatusMsg);
                            console.error(result.StatusExceptionMsg);
                        }
                    },
                    //錯誤
                    error: function (xhr, textStatus, errorThrown) {
                        //openWarningDialog(xhr.status + ' : ' + xhr.statusText);
                        console.error(xhr.responseText);
                    },
                    //之後
                    complete: function () {
                        if (loadingName != undefined) {
                            _this[loadingName] = false;
                        }
                    },
                });
            },
            Submit() {
                //檢查有題目必填但為填寫
                var _this = this;
                if (_this.CheckQuestion()) {

                    if (!_this.IsPreview) {
                        _this.ReturnAjax(
                            master.API.SaveAnswer,
                            {
                                "SurveyAnswer": encodeURIComponent(JSON.stringify(_this.ProcessAnswer())),
                                "FromString": _this.fromString
                            },
                            (res) => {
                                if (res.IsSuccess) {
                                    _this.Page = 2;
                                } else {

                                }



                            }, "IsLoading"
                        )
                    } else {
                        _this.Page = 2;
                    }


                }



            },
            ProcessAnswer() {
                var _this = this;
                var anwserMain = {
                    SurveyId: _this.SurveyId,
                    //AnswerId: "",
                    //IP: "",
                    SurveyAnswerQuestions: [],
                }

                _this.QuestionDatas.forEach(item => {

                    // 單行文字/多行文字/日期/時分/上傳檔案
                    if (item.Type == 1 || item.Type == 2 || item.Type == 5 || item.Type == 6 || item.Type == 9) {
                        anwserMain.SurveyAnswerQuestions.push({
                            SurveyId: _this.SurveyId,
                            QuestionId: item.Id,
                            AnswerId: "",
                            Type: item.Type,
                            Answer: item.Answer,
                            OtherAnswer: item.OtherAnswer
                        });
                    }
                    else if (item.Type == 3 || item.Type == 4) {
                        item.TopicOptions.forEach(q => {
                            if (q.IsActive) {
                                anwserMain.SurveyAnswerQuestions.push({
                                    SurveyId: _this.SurveyId,
                                    QuestionId: item.Id,

                                    AnswerId: "",
                                    Type: item.Type,
                                    Answer: q.Id,
                                    OtherAnswer: q.Other
                                });
                            }
                        })
                    }


                });


                return anwserMain;


            },
            CheckQuestion() {
                var _this = this;
                var Id = "";
                _this.QuestionDatas.forEach(item => {

                    if (_this.CheckRequestNeedAlert(item, true) && Id == "") {
                        Id = item.Id;
                    }
                })
                if (Id != "") {
                    var element = this.$refs[Id];
                    element[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
                    return false;
                }

                return true;
            }

        }


    })


});