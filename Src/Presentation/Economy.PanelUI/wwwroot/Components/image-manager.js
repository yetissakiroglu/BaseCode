// Tekil picker açılışı
let currentMode = null;
let currentInput = null, currentPreview = null, currentGroupIndex = -1;

function openPicker() {
    const w = 1000, h = 700;
    const y = window.top.outerHeight / 2 + window.top.screenY - (h / 2);
    const x = window.top.outerWidth / 2 + window.top.screenX - (w / 2);
    window.open('/Tenant/ImageManager/Picker', 'picker',
        `popup=yes,width=${w},height=${h},left=${x},top=${y}`);
}

// Tekil alan butonu
document.addEventListener('click', e => {
    if (e.target.closest('.btn-pick-single')) {
        const btn = e.target.closest('.btn-pick-single');
        currentMode = 'single';
        currentInput = document.querySelector(btn.dataset.targetInput);
        currentPreview = document.querySelector(btn.dataset.targetPreview);
        openPicker();
    }
    if (e.target.closest('.btn-pick-gallery')) {
        const btn = e.target.closest('.btn-pick-gallery');
        currentMode = 'gallery';
        currentGroupIndex = btn.dataset.groupIndex;
        openPicker();
    }
});

// Picker dönüşü
window.addEventListener('message', ev => {
    if (!ev.data || ev.data.type !== 'image-picked') return;
    const urls = ev.data.urls || [];
    if (!urls.length) return;

    if (currentMode === 'single') {
        const u = urls[0];
        currentInput.value = u;
        currentPreview.src = u;
        currentPreview.style.display = 'inline-block';
    }
    else if (currentMode === 'gallery') {
        urls.forEach(u => addImageToGallery(currentGroupIndex, u));
    }
});

// Galeriye resim ekleme
function addImageToGallery(gi, url) {
    const gal = document.getElementById(`gal_${gi}`);
    const col = document.createElement('div');
    col.className = "col-sm-2 col-xl-2 mb-3";
    col.innerHTML = `
        <label class="form-checkimage w-100">
            <input class="checkimage-input" type="radio" 
                   name="gallerycover_${gi}" 
                   value="${url}" 
                   data-url="${url}" data-group="${gi}">
            <span class="check-box radiobox">
              <img src="${url}" alt="" class="checkbox-image w-100 rounded shadow-sm" width="230" height="150" />
            </span>
            <button type="button" class="btn btn-sm btn-outline-danger mt-1 btn-del"
                    data-url="${url}" data-group="${gi}">Sil</button>
        </label>
        <input type="hidden" name="Galleries[${gi}].Items" value="${url}" />
    `;
    gal.appendChild(col);
}

// Radio ile kapak seçimi
document.addEventListener('change', e => {
    if (e.target.classList.contains('checkimage-input') && e.target.type === 'radio') {
        const url = e.target.dataset.url;
        const gi = e.target.dataset.group;
        document.getElementById(`Galleries_${gi}_CoverUrl`).value = url;
    }
});

// Sil
document.addEventListener('click', e => {
    if (e.target.classList.contains('btn-del')) {
        const url = e.target.dataset.url;
        const gi = e.target.dataset.group;
        const col = e.target.closest('.col-sm-2, .col-xl-2');
        if (col) col.remove();
        document.querySelectorAll(`input[name="Galleries[${gi}].Items"][value="${url}"]`).forEach(x => x.remove());
        const cover = document.getElementById(`Galleries_${gi}_CoverUrl`);
        if (cover.value === url) cover.value = '';
    }
});
