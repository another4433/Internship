// General site-wide JavaScript
document.addEventListener('DOMContentLoaded', function() {
    console.log('AQWE Restaurant System Loaded');
    
    // Add any site-wide interactive elements here
    const profileImages = document.querySelectorAll('img[data-profile]');
    
    profileImages.forEach(img => {
        img.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.05)';
            this.style.transition = 'transform 0.3s ease';
        });
        
        img.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    });
    
    // Toast notifications
    const toast = document.getElementById('toast-notification');
    if (toast) {
        setTimeout(() => {
            toast.style.opacity = '0';
            setTimeout(() => toast.remove(), 300);
        }, 3000);
    }
});