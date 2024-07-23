document.querySelectorAll('form').forEach(form => {
    form.addEventListener('submit', function () {
        document.querySelectorAll('form').forEach(f => f.querySelector('button').classList.remove('active'));
        this.querySelector('button').classList.add('active');
    });
});
