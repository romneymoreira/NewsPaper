/*
 Javascript Comum para todo o projeto.
 Referenciado na MasterPage
*/

//***************************************************
// Carregamento geral
//***************************************************

$(document).ready(function ($) {

    //Scripts Opt-in
    LoadOptInScripts();

    //Pop-Over
    $('body').on('click', function (e) {
        $('[data-toggle="popover"]').each(function () {
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('hide');
            }
        });
    });

    $(document)
      .on('show.bs.modal', '.modal', function (event) {
          $(this).appendTo($('body'));
      })
      .on('shown.bs.modal', '.modal.in', function (event) {
          setModalsAndBackdropsOrder();
      })
      .on('hidden.bs.modal', '.modal', function (event) {
          setModalsAndBackdropsOrder();
      });

    function setModalsAndBackdropsOrder() {
        var modalZIndex = 1040;
        $('.modal.in').each(function (index) {
            var $modal = $(this);
            modalZIndex++;
            $modal.css('zIndex', modalZIndex);
            $modal.next('.modal-backdrop.in').addClass('hidden').css('zIndex', modalZIndex - 1);
        });
        $('.modal.in:visible:last').focus().next('.modal-backdrop.in').removeClass('hidden');
    }
});


//***************************************************
// Função que recarrega os elementos Opt-In
//***************************************************
function LoadOptInScripts() {

    //Apply DatePickers
    applyDatePickers();

    //Hack disabled tabs
    hackDisabledTabs();

    //Toastr
    loadToastrOptions();

    //PopOvers
    loadPopOvers();

    //Tooltips
    loadTooltips();

    //BtnExcluir
    loadConfirmDelete();

    //Style check´s e radios
    loadiCheck();

    //Masks
    loadMasks();

    //Datepicker
    loadDatePicker();

    //Select2
    loadSelect2();

    //Validate
    validateForm();
}

function loadMenu() {
    setup_sidebar_menu();
}

$.fn.limiter = function (limit, elem) {
            $(this).on("keyup focus", function () {
                setCount(this, elem);
            });
            function setCount(src, elem) {
                var chars = src.value.length;
                if (chars > limit) {
                    src.value = src.value.substr(0, limit);
                    chars = limit;
                }
                elem.html(limit - chars);
            }
            setCount($(this)[0], elem);
 }

//****************************************************************
// Função para load de partial views
//****************************************************************
    
    $.fn.LoadPartial = function (opts) {

        var $target = $(this);

        var defaults = {
            action: null,            // Url action, obrigatório
            type: 'GET',             // Tipo da requisição: GET ou POST, opcional
            submitButton: null,      // Elemento submit p/ loading, opcional
            params: null,            // Parâmetros da requisição, opcional
            blockElem: null,         // Elemento para block (loading).Pode ser um elemento (ID) ou page para bloquear todo o conteúdo
            async: true              // Requisição sync/async, opcional
        }

        $.extend(defaults, opts);

        $.ajax({
            type: opts.type,
            url: opts.action,
            data: opts.params,
            async: opts.async,
            dataType: 'html',
            beforeSend: function () {
                if (opts.blockElem) {
                    if (opts.blockElem.toUpperCase() == 'PAGE')
                        blockPage();
                    else
                        blockContent(opts.blockElem);
                }
                if (opts.submitButton)
                    $(opts.submitButton).button('loading');
            }
        })
        .fail(function (jqXhr, textStatus, errorThrown) {
            toastr.error('Ocorreu um erro ao processar sua requisição. Por favor, tente novamente.');
            console.log(textStatus);
            console.log(errorThrown);
        })
        .done(function (result) {
            $target.html(result);
        })
        .always(function () {
            if (opts.blockElem) {
                if (opts.blockElem.toUpperCase() == 'PAGE')
                    unblockPage();
                else
                    unblockContent(opts.blockElem);
            }
            if (opts.submitButton)
                $(opts.submitButton).button('reset');
        });
    }


//****************************************************************
// Função para dropdown remoto
//****************************************************************

$.fn.SeekDrop = function (params) {
        
        var defaults = {
            searchUrl: "",          // Url de busca
            searchParam: "id",      // Nome do parametro de busca
            dataParams: null,       // Parametros adicionais a serem passados para a url (deve ser um objeto)
            selectUrl: "",          // Url de GetById (para abrir o registro selecionado)
            selectParam: "id",      // Nome do parametro do GetById
            placeHolder: "Buscar",  // Texto a ser exibido na caixa de texto
            minLength: 2,           // Numero minimo de caracteres para busca
            text: "text",           // Propriedade TEXT do objeto de retorno
            id: "id"                // Propriedade ID do objeto de retorno
        }

        $.extend(defaults, params);

        if (!params.searchUrl) {
            console.log('SeekDrop: Parametro "url" não foi especificado');
            return;
        }
        if (!params.selectUrl) {
            console.log('SeekDrop: Parametro "selectUrl" não foi especificado');
            return;
        }

        $(this).select2({
            placeholder: params.placeHolder,
            minimumInputLength: params.minLength,
            ajax: {
                url: params.searchUrl,
                dataType: 'json',
                quietMillis: 250,
                data: function (term, page) {
                    var jsonObj = JSON.parse('{"' + params.searchParam + '":"' + term + '"}');

                    if (isObject(params.dataParams)) {
                        jsonObj = $.extend(jsonObj, params.dataParams);
                    }
                    return $.parseJSON(JSON.stringify(jsonObj));
                },
                results: function (data, page) {
                    return {
                        results: $.map(data, function (item) {
                            return {
                                text: item[params.text],
                                id: item[params.id]
                            }
                        })
                    };
                },
                cache: true
            },
            initSelection: function (element, callback) {
                var id = $(element).val();
                if (id !== "") {
                    $.ajax(params.selectUrl + "?" + params.selectParam + "=" + id, {
                        dataType: "json"
                    }).done(function (data) {
                        callback({
                            id: data[params.id],
                            text: data[params.text]
                        });
                    });
                }
            },
            dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
        });
}

function isObject(val) {
    if (val === null) { return false;}
    return ((typeof val === 'function') || (typeof val === 'object'));
}



//****************************************************************
// Função para exibir as mensagens do toast 
//****************************************************************

function ExibirMensagens(msg, tipo) {
    var opts = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    if (tipo === "I") {//Incluído
        toastr.success(msg, "Incluído com sucesso!", opts);
    }
    else if (tipo === "E") {//Erro
        toastr.error(msg, "Ocorreu um Erro, tente novamente!");
    }
    else if (tipo === "A") {//Atualizado
        toastr.success(msg, "Atualizado com sucesso!", opts);
    }
    else if (tipo === "D") {//Excluido
        toastr.success(msg, "Excluido com sucesso!", opts);
    }
}

//****************************************************************
// Função que permite apenas numeros em inputs
//****************************************************************
function soNums(e) {

    //teclas adicionais permitidas (tab,delete,backspace,setas direita e esquerda)
    keyCodesPermitidos = new Array(8, 9, 37, 39, 46);

    //numeros e 0 a 9 do teclado alfanumerico
    for (x = 48; x <= 57; x++) {
        keyCodesPermitidos.push(x);
    }

    //numeros e 0 a 9 do teclado numerico
    for (x = 96; x <= 105; x++) {
        keyCodesPermitidos.push(x);
    }

    //Pega a tecla digitada
    keyCode = e.which;

    //Verifica se a tecla digitada é permitida
    if ($.inArray(keyCode, keyCodesPermitidos) != -1) {
        return true;
    }
    return false;
}

//***************************************************
// Função que manipula o dropdown de cidade/estado com o select2
//***************************************************

$.fn.CarregaCidades = function (opts) {

    var defaults = {
        elemUf: null,
        selectedId: null,
        selectedText: null
    }

    $.extend(defaults, opts);

    var idUfSelecionado = $(opts.elemUf).val();
    var $cidadeControl = $(this);

    $.ajax({
        type: 'GET',
        url: '/Api/CidadeApi/Get',
        data: { estadoId: idUfSelecionado },
        beforeSend: function() {
            
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        toastr.error(errorThrown, "Ocorreu um erro");
    })
    .done(function (data) {

        var $parent = $cidadeControl.parent();
        
        var selectedId = $cidadeControl.attr('data');
        if (!selectedId)
            selectedId = opts.selectedId;

        $cidadeControl.select2("destroy");
        $cidadeControl.remove();

        var cidades = eval(data);

        if (cidades && cidades.length > 0) {
            var newControl = '<select id="' + $cidadeControl.attr('id') + '" name="' + $cidadeControl.attr('id').replace(/_/g, ".") + '" class="select2 entity-dropdown">';

            for (var i = 0; i < cidades.length; i++) {
                newControl += '<option value="' + cidades[i].IdCidade + '"';
                if ((selectedId && cidades[i].IdCidade == selectedId) || 
                    (opts.selectedText && cidades[i].Nome == opts.selectedText)) 
                {
                    newControl += 'selected';
                }
                    
                newControl += '>' + cidades[i].Nome + '</option>';
            }

            newControl += '</select>';
            $(newControl).appendTo($parent);

            ApplySelect2('#' + $cidadeControl.attr('id'));
        }
    })
    .always(function() {
        
    });
};

function carregaCidades(eUf, eCidade) {
    var $stateControl = $(eUf);
    var selectedStateId = $stateControl.val();

    if (selectedStateId != null && selectedStateId > 0) {
        $.ajax({
            type: 'GET',
            url: '/Api/CidadeApi/Get',
            //async: false,
            data: { estadoId: selectedStateId }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            var opts = {
                "closeButton": true,
                "debug": false,
                "positionClass": "toast-top-full-width",
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "0",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
            toastr.error(errorThrown, "Erro, atualize a página, por favor.", opts);
        }).done(function (data) {
            callbackCarregaCidades(eCidade, data);
        });
    }
}


function callbackCarregaCidades(e, data) {
    var $cidadeControl = $(e);
    var $parent = $cidadeControl.parent();

    var selectedId = $cidadeControl.attr('data');

    $cidadeControl.select2("destroy");
    $cidadeControl.remove();

    var cidades = eval(data);

    if (cidades && cidades.length > 0) {
        var newControl = '<select id="' + e.replace('#', '') + '" name="' + e.replace(/_/g, ".").replace('#', '') + '" class="select2 entity-dropdown">';

        for (var i = 0; i < cidades.length; i++) {
            newControl += '<option value="' + cidades[i].IdCidade + '" ' + (cidades[i].IdCidade == selectedId ? 'selected' : '') + '>' + cidades[i].Nome + '</option>';
        }

        newControl += '</select>';

        $(newControl).appendTo($parent);

        ApplySelect2(e);
    }
}

//***************************************************
// Função para validar hora inicial/final
//***************************************************
function isHoraInicialMenorHoraFinal(horaInicial, horaFinal) {
    var horaIni = horaInicial.split(':');
    var horaFim = horaFinal.split(':');

    // Verifica as horas. Se forem diferentes, é só ver se a inicial
    // é menor que a final.
    var hIni = parseInt(horaIni[0], 10);
    var hFim = parseInt(horaFim[0], 10);
    if (hIni != hFim)
        return hIni < hFim;


    // Se as horas são iguais, verifica os minutos então.
    var mIni = parseInt(horaIni[1], 10);
    var mFim = parseInt(horaFim[1], 10);
    if (mIni != mFim)
        return mIni < mFim;

    return false;

}



//**********************************
// Função que aplica o datepicker
//**********************************
function applyDatePickers() {
    // Datepicker
    if ($.isFunction($.fn.datepicker)) {
        $(".datepicker").each(function (i, el) {
            var $this = $(el),
                opts = {
                    format: attrDefault($this, 'format', 'mm/dd/yyyy'),
                    startDate: attrDefault($this, 'startDate', ''),
                    endDate: attrDefault($this, 'endDate', ''),
                    daysOfWeekDisabled: attrDefault($this, 'disabledDays', ''),
                    startView: attrDefault($this, 'startView', 0),
                    rtl: rtl()
                },
                $n = $this.next(),
                $p = $this.prev();

            $this.datepicker(opts);

            if ($n.is('.input-group-addon') && $n.has('a')) {
                $n.on('click', function (ev) {
                    ev.preventDefault();

                    $this.datepicker('show');
                });
            }

            if ($p.is('.input-group-addon') && $p.has('a')) {
                $p.on('click', function (ev) {
                    ev.preventDefault();

                    $this.datepicker('show');
                });
            }
        });
    }
}

//***************************************************
// Função que aplica máscaras para formulários carregados via AJAX
//***************************************************
function applyMasks() {
    // Input Mask
    if ($.isFunction($.fn.inputmask)) {
        $("[data-mask]").each(function (i, el) {
            var $this = $(el),
                mask = $this.data('mask').toString(),
                opts = {
                    numericInput: attrDefault($this, 'numeric', false),
                    radixPoint: attrDefault($this, 'radixPoint', ''),
                    rightAlignNumerics: attrDefault($this, 'numericAlign', 'left') == 'right'
                },
                placeholder = attrDefault($this, 'placeholder', ''),
                is_regex = attrDefault($this, 'isRegex', '');


            if (placeholder.length) {
                opts[placeholder] = placeholder;
            }

            switch (mask.toLowerCase()) {
                case "telefone":
                    mask = "(99) 9999-9999[9]";
                    break;

                case "currency":
                case "rcurrency":

                    var sign = attrDefault($this, 'sign', 'R$');;

                    mask = "999.999.999,99";

                    if ($this.data('mask').toLowerCase() == 'rcurrency') {
                        mask += ' ' + sign;
                    }
                    else {
                        mask = sign + ' ' + mask;
                    }

                    opts.numericInput = true;
                    opts.rightAlignNumerics = false;
                    opts.radixPoint = ',';
                    break;

                case "email":
                    mask = 'Regex';
                    opts.regex = "[a-zA-Z0-9._%-]+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,4}";
                    break;

                case "fdecimal":
                    mask = 'decimal';
                    $.extend(opts, {
                        autoGroup: true,
                        groupSize: 3,
                        digits: attrDefault($this, 'decimals', '2'),
                        radixPoint: attrDefault($this, 'rad', ','),
                        groupSeparator: attrDefault($this, 'dec', '.')
                    });
                    break;

                case "cpf":
                    mask = "999.999.999-99";
                    break;

                case "cnpj":
                    mask = "99.999.999/9999-99";
                    break;

                case "cep":
                    mask = "99999-999";
                    break;
            }

            if (is_regex) {
                opts.regex = mask;
                mask = 'Regex';
            }

            $this.inputmask(mask, opts);
        });
    }
}

function setMasks(element) {
    // Input Mask
    if ($.isFunction($.fn.inputmask)) {

        var $this = $(element),
            mask = $this.data('mask').toString(),
            opts = {
                numericInput: attrDefault($this, 'numeric', false),
                radixPoint: attrDefault($this, 'radixPoint', ''),
                rightAlignNumerics: attrDefault($this, 'numericAlign', 'left') == 'right'
            },
            placeholder = attrDefault($this, 'placeholder', ''),
            is_regex = attrDefault($this, 'isRegex', '');


        if (placeholder.length) {
            opts[placeholder] = placeholder;
        }

        switch (mask.toLowerCase()) {
            case "telefone":
                mask = "(99) 9999-9999[9]";
                break;

            case "currency":
            case "rcurrency":

                var sign = attrDefault($this, 'sign', 'R$');;

                mask = "999.999.999,99";

                if ($this.data('mask').toLowerCase() == 'rcurrency') {
                    mask += ' ' + sign;
                }
                else {
                    mask = sign + ' ' + mask;
                }

                opts.numericInput = true;
                opts.rightAlignNumerics = false;
                opts.radixPoint = ',';
                break;

            case "email":
                mask = 'Regex';
                opts.regex = "[a-zA-Z0-9._%-]+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,4}";
                break;

            case "fdecimal":
                mask = 'decimal';
                $.extend(opts, {
                    autoGroup: true,
                    groupSize: 3,
                    digits: attrDefault($this, 'decimals', '2'),
                    radixPoint: attrDefault($this, 'rad', ','),
                    groupSeparator: attrDefault($this, 'dec', '.')
                });
                break;

            case "cpf":
                mask = "999.999.999-99";
                break;

            case "cnpj":
                mask = "99.999.999/9999-99";
                break;

            case "cep":
                mask = "99999-999";
                break;
        }

        if (is_regex) {
            opts.regex = mask;
            mask = 'Regex';
        }

        $this.inputmask(mask, opts);

    }
}

//***************************************************
// Função que inicializa o DataTables em um Elemento
//***************************************************
function getDtLanguageObject() {
    var lang = {
        sLengthMenu: "Mostrar _MENU_",
        sEmptyTable: "Nenhum registro a exibir.",
        sSearch: "Filtrar",
        sInfo: "Exibindo de _START_ a _END_ (_TOTAL_ no grid)",
        sInfoEmpty: "0 registros."
    }

    return lang;
}

function initDataTables(selector) {
    var table = document.getElementById(selector);
    if (!$.fn.dataTable.fnIsDataTable(table)) {
        table = $(selector).dataTable({
            "sPaginationType": "bootstrap",
            "aLengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
            "oLanguage": getDtLanguageObject(),
            "bRetrieve": true
        });
    }

    return table;
}

function initDataTablesOpts(selector, opts) {
    var table = document.getElementById(selector);
    if (!$.fn.dataTable.fnIsDataTable(table)) {
        var defaultParams = {
            "sPaginationType": "bootstrap",
            "aLengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
            "oLanguage": getDtLanguageObject()
        }
        opts = $.extend(defaultParams, opts);
        table = $(selector).dataTable(opts);
    }

    return table;
}

function initDataTablesCustom(selector, enablePagination, pageSize, showSearch, showPageSize, showFooter) {
    var tableIniter = $(selector).dataTable({
        "oLanguage": getDtLanguageObject(),
        "sPaginationType": "bootstrap",
        "iDisplayLength": pageSize,
        "aLengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
        "bPaginate": enablePagination,
        "bFilter": showSearch,
        "bLengthChange": showPageSize,
        "bInfo": showFooter
    });

    return tableIniter;
}

//***************************************************
// Modal Dinamico
//***************************************************
$(document).on('click', '.dynamic-modal', function (e) {
    e.preventDefault();
    var sender = $(this);
    var target = $(sender).attr('href');
    var modalSize = $(sender).attr('data-size');
    var modalOver = $(sender).attr('data-stack');
    var jsonStr = $(sender).attr('data-object');
    var dataObj = null;

    var modalID = "#dynamic-modal";
    var modalContentID = "#dynamic-modal-content";
    var modalDialogID = "#dynamic-modal-dialog";

    if (jsonStr != null) {
        jsonStr = replaceAll("'", "\"", jsonStr);
        jsonStr = '{' + jsonStr + '}';
        var jsonObj = $.parseJSON(jsonStr);
        var dataObj = $.param(jsonObj);
    }

    if (modalOver) {
        modalID += "-stack";
        modalContentID += "-stack";
        modalDialogID += "-stack";
    }

    $.ajax({
        url: target,
        type: "GET",
        dataType: "html",
        data: dataObj,
        beforeSend: function () {
            blockPage();
        },
        complete: function () {
            unblockPage();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        },
        success: function (result) {
            $(modalContentID).html(result);
            $(modalID).modal({
                backdrop: 'static',
                keyboard: true
            });

            LoadOptInScripts();

            $(modalDialogID).css({
                width: modalSize
            });

            $("#dynamic-modal").scroll(function () {
                $('.datepicker').datepicker('hide');
                $('.button-datepicker').datepicker('hide');
            });
        }
    });
});

//$(document).on("showModalSettings", showModalSettingsHandler);
//function showModalSettingsHandler(e) {
//    console.log(e);
//}
//$.event.trigger("eventDone");

$(document).on('click', '.static-modal', function (e) {
    e.preventDefault();
    var sender = $(this);
    var target = $(sender).attr('href');
    var callback = $(sender).attr('callback');
    var modalSize = $(sender).attr('data-size');

    var staticHtml = $(target).html();

    $("#static-modal-content").html(staticHtml);
    $("#static-modal").modal({
        backdrop: 'static',
        keyboard: true
    });

    LoadOptInScripts();

    $("#static-modal-dialog").css({
        width: modalSize
    });

    $("#static-modal").scroll(function () {
        $('.datepicker').datepicker('hide');
        $('.button-datepicker').datepicker('hide');
    });
    if (callback && callback.length > 0)
        window[callback]();
});

function replaceAll(find, replace, str) {
    return str.replace(new RegExp(find, 'g'), replace);
}

function closeModal(modalID) {
    if (modalID != null)
        $('#' + modalID).modal('hide');
    else {
        $('#dynamic-modal').modal('hide');
        $('#static-modal').modal('hide');
    }

}

//***************************************************
// Hack para tabs desativados
//***************************************************
function hackDisabledTabs() {
    $(".nav-tabs a[data-toggle=tab]").on("click", function (e) {
        if ($(this).hasClass("disabled")) {
            e.preventDefault();
            return false;
        }
    });
    $(".nav-tabs li.disabled").on("click", function (e) {
        if ($(this).hasClass("disabled")) {
            e.preventDefault();
            return false;
        }
    });
}

//***************************************************
// iCheck
//***************************************************
function loadiCheck() {
    $('input.icheck').iCheck({
        checkboxClass: 'icheckbox_flat-blue',
        radioClass: 'iradio_flat-blue'
    });
}

//***************************************************
// Tooltip
//***************************************************
function loadTooltips() {
    $("[data-toggle='tooltip']").tooltip();
}

//***************************************************
// Modal de confirmação para exclusão
//***************************************************

function loadConfirmDelete() {
    $(document).on("click", ".btn-table-delete", function (e) {
        e.preventDefault();
        var href = $(this).attr("href");
        var attr = $(this).attr('data-msg');
        var message = 'Confirma exclusão?';
        console.log(attr);
        if (attr)
            message = attr;

        Messi.ask(message, function (val) {
            if (val)
                window.location.href = href;
        });
    });
}

//***************************************************
// Toastr
//***************************************************
function loadPopOvers() {
    $('[data-toggle="popover"]').each(function (i, el) {
        var $this = $(el),
            placement = attrDefault($this, 'placement', 'right'),
            trigger = attrDefault($this, 'trigger', 'click'),
            popover_class = $this.hasClass('popover-secondary') ? 'popover-secondary' : ($this.hasClass('popover-primary') ? 'popover-primary' : ($this.hasClass('popover-default') ? 'popover-default' : ''));

        $this.popover({
            placement: placement,
            trigger: trigger
        });

        $this.on('shown.bs.popover', function (ev) {
            var $popover = $this.next();

            $popover.addClass(popover_class);
        });
    });
}
//***************************************************
// Toastr
//***************************************************
function loadToastrOptions() {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "positionClass": "toast-top-right",
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "500",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

//***************************************************
// Masks
//***************************************************
function loadMasks() {
    $("[data-mask]").each(function (i, el) {
        var $this = $(el),
            mask = $this.data('mask').toString(),
            opts = {
                numericInput: attrDefault($this, 'numeric', false),
                radixPoint: attrDefault($this, 'radixPoint', ''),
                rightAlignNumerics: attrDefault($this, 'numericAlign', 'left') == 'right'
            },
            placeholder = attrDefault($this, 'placeholder', ''),
            is_regex = attrDefault($this, 'isRegex', '');


        if (placeholder.length) {
            opts[placeholder] = placeholder;
        }

        switch (mask.toLowerCase()) {
            case "telefone":
                mask = "(99) 9999-9999[9]";
                break;

            case "currency":
            case "rcurrency":

                var sign = attrDefault($this, 'sign', 'R$');;

                mask = "999.999.999,99";

                if ($this.data('mask').toLowerCase() == 'rcurrency') {
                    mask += ' ' + sign;
                }
                else {
                    mask = sign + ' ' + mask;
                }

                opts.numericInput = true;
                opts.rightAlignNumerics = false;
                opts.radixPoint = ',';
                break;

            case "email":
                mask = 'Regex';
                opts.regex = "[a-zA-Z0-9._%-]+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,4}";
                break;

            case "fdecimal":
                mask = 'decimal';
                $.extend(opts, {
                    autoGroup: true,
                    groupSize: 3,
                    digits: attrDefault($this, 'decimals', '2'),
                    radixPoint: attrDefault($this, 'rad', ','),
                    groupSeparator: attrDefault($this, 'dec', '.')
                });
                break;

            case "cpf":
                mask = "999.999.999-99";
                break;

            case "cnpj":
                mask = "99.999.999/9999-99";
                break;

            case "cep":
                mask = "99999-999";
                break;
        }

        if (is_regex) {
            opts.regex = mask;
            mask = 'Regex';
        }

        $this.inputmask(mask, opts);
    });
}

function loadMasksContext(context) {
    $(context + " [data-mask]").each(function (i, el) {
        var $this = $(el),
            mask = $this.data('mask').toString(),
            opts = {
                numericInput: attrDefault($this, 'numeric', false),
                radixPoint: attrDefault($this, 'radixPoint', ''),
                rightAlignNumerics: attrDefault($this, 'numericAlign', 'left') == 'right'
            },
            placeholder = attrDefault($this, 'placeholder', ''),
            is_regex = attrDefault($this, 'isRegex', '');


        if (placeholder.length) {
            opts[placeholder] = placeholder;
        }

        switch (mask.toLowerCase()) {
            case "telefone":
                mask = "(99) 9999-9999[9]";
                break;

            case "currency":
            case "rcurrency":

                var sign = attrDefault($this, 'sign', 'R$');

                mask = "999.999.999,99";

                if ($this.data('mask').toLowerCase() == 'rcurrency') {
                    mask += ' ' + sign;
                }
                else {
                    mask = sign + ' ' + mask;
                }

                opts.numericInput = true;
                opts.rightAlignNumerics = false;
                opts.radixPoint = ',';
                break;

            case "email":
                mask = 'Regex';
                opts.regex = "[a-zA-Z0-9._%-]+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,4}";
                break;

            case "fdecimal":
                mask = 'decimal';
                $.extend(opts, {
                    autoGroup: true,
                    groupSize: 3,
                    digits: attrDefault($this, 'decimals', '2'),
                    radixPoint: attrDefault($this, 'rad', ','),
                    groupSeparator: attrDefault($this, 'dec', '.')
                });
                break;

            case "cpf":
                mask = "999.999.999-99";
                break;

            case "cnpj":
                mask = "99.999.999/9999-99";
                break;

            case "cep":
                mask = "99999-999";
                break;
        }

        if (is_regex) {
            opts.regex = mask;
            mask = 'Regex';
        }

        $this.inputmask(mask, opts);
    });
}
//***************************************************
// DatePicker
//***************************************************
function loadDatePicker() {
    $(".datepicker").each(function (i, el) {
        var $this = $(el),
            opts = {
                format: attrDefault($this, 'format', 'mm/dd/yyyy'),
                startDate: attrDefault($this, 'startDate', ''),
                endDate: attrDefault($this, 'endDate', ''),
                daysOfWeekDisabled: attrDefault($this, 'disabledDays', ''),
                startView: attrDefault($this, 'startView', 0),
                rtl: rtl()
            },
            $n = $this.next(),
            $p = $this.prev();

        $this.datepicker('remove');
        $this.datepicker(opts);

        if ($n.is('.input-group-addon') && $n.has('a')) {
            $n.on('click', function (ev) {
                ev.preventDefault();

                $this.datepicker('show');
            });
        }

        if ($p.is('.input-group-addon') && $p.has('a')) {
            $p.on('click', function (ev) {
                ev.preventDefault();

                $this.datepicker('show');
            });
        }
    });


    $(".button-datepicker").each(function (i, el) {
        $(el).datepicker('remove');
    });

    $picker = $(".button-datepicker");
    $picker.datepicker({
        format: 'dd/mm/yyyy'
    }).on('changeDate', function (ev) {
        var date = moment(ev.date).format("DD/MM/YYYY");
        $picker.parent().parent().prev('input').val(date);
    });
}

//***************************************************
// Select2
//***************************************************

function loadSelect2() {
    if ($.isFunction($.fn.select2)) {
        $(".select2.form-select").each(function (i, el) {
            var $this = $(el),
                opts = {
                    allowClear: attrDefault($this, 'allowClear', false)
                };

            $this.select2(opts);
            $this.addClass('visible');
        });


        if ($.isFunction($.fn.niceScroll)) {
            $(".select2-results").niceScroll({
                cursorcolor: '#d4d4d4',
                cursorborder: '1px solid #ccc',
                railpadding: { right: 3 }
            });
        }
    }
}

function ApplySelect2(elem) {
    if ($.isFunction($.fn.select2)) {
        var $this = $(elem),
            opts = {
                allowClear: attrDefault($this, 'allowClear', false)
            };

        $this.select2(opts);
        $this.addClass('visible');

        if ($.isFunction($.fn.niceScroll)) {
            $(elem + ".select2-results").niceScroll({
                cursorcolor: '#d4d4d4',
                cursorborder: '1px solid #ccc',
                railpadding: { right: 3 }
            });
        }
    }
}
//***************************************************
// BlockUI
//***************************************************
function blockContent(element) {
    var el = $(element);
    el.block({
        message: '<img src="/erp/Regulacao/Assets/images/loader.gif" alt="">',
        centerY: true,
        css: {
            top: '10%',
            border: 'none',
            padding: '2px',
            backgroundColor: 'none',
            cursor: 'arrow'
            
            
        },
        overlayCSS: {
            backgroundColor: '#fff',
            opacity: 0.7,
            cursor: 'arrow'
          
        }
    });
}

function unblockContent(el) {
    $(el).unblock();
}

function blockModal() {
    blockContent("#dynamic-modal-content");
}
function unblockModal() {
    unblockContent("#dynamic-modal-content");
}

function blockPage() {
    blockContent(".main-content");
}

function unblockPage() {
    unblockContent(".main-content");
}

//***************************************************
// Jquery Validation
//***************************************************
function validateForm(formID, ignoreHidden) {
    //Validate
    var opts = {
        rules: {},
        messages: {},
        errorElement: 'span',
        errorClass: 'validate-has-error',
        ignore: "",
        highlight: function (element) {
            $(element).closest('.form-group').addClass('validate-has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('validate-has-error');
        },
        errorPlacement: function (error, element) {
            if (element.closest('.has-switch').length) {
                error.insertAfter(element.closest('.has-switch'));
            }
            else
                if (element.parent('.checkbox, .radio').length || element.parent('.input-group').length) {
                    error.insertAfter(element.parent());
                }
                else {
                    error.insertAfter(element);
                }
        }
    }

    if (formID == null) {
        $("form.validate").each(function () {
            $(this).validate(opts);
        });
    }
    else {
        if (ignoreHidden)
            opts.ignore = ":hidden";
        $(formID).validate(opts);
    }
}

function clearValidation(formID) {
    if (formID == null) {
        $("form.validate").each(function () {
            $(this).resetValidation();
        });
    }
    else {
        $(formID).resetValidation();
    }
}

$.fn.resetValidation = function () {
    var validator = $(this).validate();
    var inputs = validator.elements();
    $(inputs).each(function () {
        $(this).closest('.form-group').removeClass('validate-has-error');
    })
    validator.resetForm();
};

//***************************************************
// Colorable
//***************************************************
function buildColorable(color, text) {
    var cssColor;
    var html;

    switch (color) {
        case "Default":
            cssColor = "badge";
            break;
        case "Primary":
            cssColor = "badge badge-primary";
            break;
        case "Secondary":
            cssColor = "badge badge-secondary";
            break;
        case "Red":
            cssColor = "badge badge-danger";
            break;
        case "Blue":
            cssColor = "badge badge-info";
            break;
        case "Green":
            cssColor = "badge badge-success";
            break;
        case "Yellow":
            cssColor = "badge badge-warning";
            break;
        default:
            cssColor = "badge";
            break;
    }

    html = "<span class=\"" + cssColor + "\">" + text + "</span>";
    return html;
}
//***************************************************
// Alerta erro geral
//***************************************************
function AlertaErroGeral() {
    toastr.error("Ocorreu um erro ao processar sua requisição. Por favor, contate a TI", "Falha Geral");
}

//***************************************************
// Alertas
//***************************************************
function alertError(target, errorObject) {

    if ($.isArray(errorObject))
        errorObject = { Message: errorObject }
    if (typeof errorObject == 'string' || errorObject instanceof String)
        errorObject = { Message: errorObject }

    var html;
    $(target + " .alert-danger").remove();
    if ($.isArray(errorObject.Message)) {
        html = "<div class=\"alert alert-danger\"><i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;Por favor, corrija os erros abaixo:";
        html += "<ul>";
        $.each(errorObject.Message, function (i, item) {
            html += "<li>" + item + "</li>";
        });
        html += "</ul></div>";
    }
    else {
        html = "<div class=\"alert alert-danger\"><i class=\"fa fa-exclamation-triangle\"></i>&nbsp;" + errorObject.Message + "</div>";
    }

    $(target).prepend(html);
    if (errorObject.StackTrace)
        console.log(errorObject.StackTrace);
}

function removeAlerts(target) {
    $(target + " .alert").remove();
}

function alertMessage(target, opts) {
    if (!opts) {
        opts = {};
    }

    var settings = $.extend({
        title: null,
        type: "info",
        icon: "fa fa-info-circle",
        message: "",
    }, opts);
}

//********************************************************************
// Função que converte o decimal para utilização de campos com masks
//********************************************************************

function ToDecimalMask(value) {
    return value.toString().replace(".", ",");
}

//***************************************************
// Funções Moeda 
//***************************************************
function MascaraMoeda(objTextBox, SeparadorMilesimo, SeparadorDecimal, e) {

    var sep = 0;
    var key = '';
    var i = j = 0;
    var len = len2 = 0;
    var strCheck = '0123456789';
    var aux = aux2 = '';
    var whichCode = (window.Event) ? e.which : e.keyCode;
    if (whichCode == 13) return true;
    key = String.fromCharCode(whichCode); // Valor para o código da Chave
    if (strCheck.indexOf(key) == -1) return false; // Chave inválida
    len = objTextBox.value.length;
    for (i = 0; i < len; i++)
        if ((objTextBox.value.charAt(i) != '0') && (objTextBox.value.charAt(i) != SeparadorDecimal)) break;
    aux = '';
    for (; i < len; i++)
        if (strCheck.indexOf(objTextBox.value.charAt(i)) != -1) aux += objTextBox.value.charAt(i);
    aux += key;
    len = aux.length;
    if (len == 0) objTextBox.value = '';
    if (len == 1) objTextBox.value = '0' + SeparadorDecimal + '0' + aux;
    if (len == 2) objTextBox.value = '0' + SeparadorDecimal + aux;
    if (len == 4) objTextBox.value = '0' + SeparadorDecimal + aux;
    if (len > 2) {
        aux2 = '';
        for (j = 0, i = len - 3; i >= 0; i--) {
            if (j == 3) {
                aux2 += SeparadorMilesimo;
                j = 0;
            }
            aux2 += aux.charAt(i);
            j++;
        }
        objTextBox.value = '';
        len2 = aux2.length;
        for (i = len2 - 1; i >= 0; i--)
            objTextBox.value += aux2.charAt(i);
        objTextBox.value += SeparadorDecimal + aux.substr(len - 2, len);
    }
    return false;
}

function convertMoeda(moeda) {
    moeda = moeda.replace(".", "");
    moeda = moeda.replace(",", ".");
    return parseFloat(moeda);
}

function float2moeda(num) {
    x = 0;
    if (num < 0) {
        num = Math.abs(num);
        x = 1;
    }
    if (isNaN(num)) num = "0";
    cents = Math.floor((num * 100 + 0.5) % 100);
    num = Math.floor((num * 100 + 0.5) / 100).toString();

    if (cents < 10) cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + '.'
              + num.substring(num.length - (4 * i + 3));
    ret = num + ',' + cents;
    if (x == 1) ret = ' - ' + ret; return ret;
}


//***************************************************
// Limpa Formulário
//***************************************************
function limpaFormulario(formulario) {
    $(formulario).each(function () {
        this.reset();
    });
}

//***************************************************
// Função Spinner
//***************************************************
function setSpinner() {
   
    $(".input-spinner").each(function (i, el) {
        var $this = $(el),
            $minus = $this.find('button:first'),
            $plus = $this.find('button:last'),
            $input = $this.find('input'),

            minus_step = attrDefault($minus, 'step', -1),
            plus_step = attrDefault($minus, 'step', 1),

            min = attrDefault($input, 'min', null),
            max = attrDefault($input, 'max', null);


        $this.find('button').on('click', function (ev) {
            ev.preventDefault();

            var $this = $(this),
                val = $input.val(),
                step = attrDefault($this, 'step', $this[0] == $minus[0] ? -1 : 1);

            if (!step.toString().match(/^[0-9-\.]+$/)) {
                step = $this[0] == $minus[0] ? -1 : 1;
            }

            if (!val.toString().match(/^[0-9-\.]+$/)) {
                val = 0;
            }

            $input.val(parseFloat(val) + step).trigger('keyup');
        });

        $input.keyup(function () {
            if (min != null && parseFloat($input.val()) < min) {
                $input.val(min);
            }
            else

                if (max != null && parseFloat($input.val()) > max) {
                    $input.val(max);
                }
        });
    });
}

//***************************************************
// Validação de CPF e CNPJ
//***************************************************
function validaCpf(str) {
    str = str.replace('.', '');
    str = str.replace('.', '');
    str = str.replace('-', '');

    cpf = str;
    var numeros, digitos, soma, i, resultado, digitos_iguais;
    digitos_iguais = 1;
    if (cpf.length < 11)
        return false;
    for (i = 0; i < cpf.length - 1; i++)
        if (cpf.charAt(i) != cpf.charAt(i + 1)) {
            digitos_iguais = 0;
            break;
        }
    if (!digitos_iguais) {
        numeros = cpf.substring(0, 9);
        digitos = cpf.substring(9);
        soma = 0;
        for (i = 10; i > 1; i--)
            soma += numeros.charAt(10 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0))
            return false;
        numeros = cpf.substring(0, 10);
        soma = 0;
        for (i = 11; i > 1; i--)
            soma += numeros.charAt(11 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1))
            return false;
        return true;
    }
    else
        return false;
}

function validarCNPJ(cnpj) {

    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14)
        return false;

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999")
        return false;

    // Valida DVs
    tamanho = cnpj.length - 2;
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0))
        return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1))
        return false;

    return true;

}

//***************************************************
// Validação Hora Inicial/Final
//***************************************************

function isHoraInicialMenorHoraFinal(horaInicial, horaFinal) {
    horaIni = horaInicial.split(':');
    horaFim = horaFinal.split(':');

    // Verifica as horas. Se forem diferentes, é só ver se a inicial 
    // é menor que a final.
    hIni = parseInt(horaIni[0]);
    hFim = parseInt(horaFim[0]);
    if (hIni == hFim) {
        mIni = parseInt(horaIni[1]);
        mFim = parseInt(horaFim[1]);
        if (mIni == mFim) {
            return false;
        }
        else if (mIni < mFim) {
            return true;
        }
        else {
            return false;
        }
    }
    else if (hIni > hFim) {
        return false;
    }
    else if (hIni < hFim) {
        return true;
    }
}

//***************************************************
// Calcula Porcentagem sobre um valor
//***************************************************
function calcularPorcentagem(valorTotal, valorInformado) {
    valorTotal = parseFloat(valorTotal.toString().replace(',', '.'));
    valorInformado = parseFloat(valorInformado.toString().replace(',', '.'));

    var percentual = ((valorInformado * 100) / valorTotal) - 100;

    return percentual.replace('.', ',') + "%";
}