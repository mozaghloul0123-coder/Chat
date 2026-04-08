const API_BASE = '/api/auth';

function switchTab(tab) {
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    const tabs = document.querySelectorAll('.tab-btn');

    if (tab === 'login') {
        loginForm.classList.remove('hidden');
        registerForm.classList.add('hidden');
        tabs[0].classList.add('active');
        tabs[1].classList.remove('active');
    } else {
        loginForm.classList.add('hidden');
        registerForm.classList.remove('hidden');
        tabs[0].classList.remove('active');
        tabs[1].classList.add('active');
    }
}

function showStatus(msg, isError = false) {
    const box = document.getElementById('statusMessage');
    box.textContent = msg;
    box.className = `status-box ${isError ? 'status-error' : 'status-success'}`;
    box.classList.remove('hidden');
}

// Login
document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const email = document.getElementById('loginEmail').value;
    const password = document.getElementById('loginPassword').value;

    try {
        const resp = await fetch(`${API_BASE}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });

        const data = await resp.json();
        if (resp.ok) {
            handleAuthSuccess(data);
        } else {
            showStatus(data.message || 'Login failed', true);
        }
    } catch (err) {
        showStatus('Server connection error', true);
    }
});

// Register
document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const userName = document.getElementById('regUser').value;
    const email = document.getElementById('regEmail').value;
    const password = document.getElementById('regPassword').value;

    try {
        const resp = await fetch(`${API_BASE}/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userName, email, password })
        });

        const data = await resp.json();
        if (resp.ok) {
            showStatus('Account created! Sign in now.');
            switchTab('login');
        } else {
            showStatus(JSON.stringify(data), true);
        }
    } catch (err) {
        showStatus('Registration error', true);
    }
});

function handleAuthSuccess(data) {
    localStorage.setItem('accessToken', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    
    document.getElementById('userName').textContent = "Authenticated User";
    document.getElementById('tokenDisplay').textContent = data.accessToken;
    document.getElementById('userPanel').classList.remove('hidden');
    document.querySelector('.auth-card').classList.add('hidden');
    document.querySelector('.hero-section').classList.add('hidden');
}

async function testAdmin() {
    const token = localStorage.getItem('accessToken');
    const respArea = document.getElementById('adminResponse');
    
    try {
        const resp = await fetch('/api/admin/dashboard', {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (resp.status === 403) {
            respArea.innerHTML = '<span style="color:red">Access Denied! You are not an Admin.</span>';
        } else if (resp.ok) {
            const data = await resp.json();
            respArea.innerHTML = `<span style="color:#10b981">Success: ${data.message}</span>`;
        } else {
            respArea.textContent = 'Error: ' + resp.status;
        }
    } catch (err) {
        respArea.textContent = 'Network Error';
    }
}

function logout() {
    localStorage.clear();
    location.reload();
}
