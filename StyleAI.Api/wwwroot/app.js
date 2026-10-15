const fileInput = document.getElementById('fileInput');
const submitBtn = document.getElementById('submitBtn');
const uploadForm = document.getElementById('uploadForm');
const resultSection = document.getElementById('result');
const styleNameEl = document.getElementById('styleName');
const confidenceEl = document.getElementById('confidence');
const paletteEl = document.getElementById('palette');
const summaryEl = document.getElementById('summary');
const clothingEl = document.getElementById('clothing');
const hobbiesEl = document.getElementById('hobbies');
const messageEl = document.getElementById('message');

fileInput.addEventListener('change', () => {
  submitBtn.disabled = !fileInput.files.length;
});

uploadForm.addEventListener('submit', async (ev) => {
  ev.preventDefault();
  messageEl.textContent = '';
  resultSection.classList.add('hidden');

  const file = fileInput.files[0];
  if (!file) return;

  submitBtn.disabled = true;
  submitBtn.textContent = 'Анализируется...';

  try {
    const form = new FormData();
    form.append('file', file, file.name);

    const resp = await fetch('/api/ml/predict', {
      method: 'POST',
      body: form
    });

    if (!resp.ok) {
      const txt = await resp.text();
      throw new Error(txt || 'Ошибка сервера');
    }

    const data = await resp.json();

    // Present results without mentioning internal implementation
    styleNameEl.textContent = data.style_name || data.style_id || '—';
    confidenceEl.textContent = `Уверенность: ${Math.round((data.confidence||0)*100)}%`;

    // palette may be present in styles via /styles.json; try to read reco fields
    const reco = data.recommendation || null;

    // If server returns all_probs and styles, we can display palette from matching style
    if (!reco && data.all_probs) {
      // attempt to fetch styles table from /python_ml/styles.json is not exposed; use basic view
    }

    // If server included recommendation object, render it; otherwise fallback to summary from API
    if (reco) {
      summaryEl.textContent = reco.summary || '';
      clothingEl.textContent = 'Одежда: ' + (reco.clothing || []).join(', ');
      hobbiesEl.textContent = 'Хобби: ' + (reco.hobbies || []).join(', ');
      renderPalette(reco.palette || []);
    } else if (data.all_probs && Array.isArray(data.all_probs)) {
      // unlikely shape; just clear
      summaryEl.textContent = '';
      clothingEl.textContent = '';
      hobbiesEl.textContent = '';
      renderPalette([]);
    } else {
      // try to request style catalog (not available through API now) — fallback
      summaryEl.textContent = '';
      clothingEl.textContent = '';
      hobbiesEl.textContent = '';
      renderPalette(data.palette || []);
    }

    resultSection.classList.remove('hidden');
  } catch (err) {
    console.error(err);
    messageEl.textContent = 'Ошибка: ' + (err.message || err);
  } finally {
    submitBtn.disabled = false;
    submitBtn.textContent = 'Анализировать';
  }
});

function renderPalette(colors) {
  paletteEl.innerHTML = '';
  if (!colors || !colors.length) return;
  colors.forEach(c => {
    const el = document.createElement('div');
    el.className = 'swatch';
    el.style.background = c;
    paletteEl.appendChild(el);
  });
}
