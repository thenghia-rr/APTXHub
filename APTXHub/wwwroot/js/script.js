 // On page load or when changing themes, best to add inline in `head` to avoid FOUC
//if (localStorage.theme === 'dark' || (!('theme' in localStorage))) {
//    document.documentElement.classList.add('light') // Doi nguoc lai de TEST
//} else {
//    document.documentElement.classList.remove('dark')
//}

//// Whenever the user explicitly chooses light mode
//localStorage.theme = 'light'

//// Whenever the user explicitly chooses dark mode
//localStorage.theme = 'dark'

//// Whenever the user explicitly chooses to respect the OS preference
//localStorage.removeItem('theme')



//// add post upload image
//document.getElementById('addPostUrl').addEventListener('change', function () {
//    if (this.files[0]) {
//        var picture = new FileReader();
//        picture.readAsDataURL(this.files[0]);
//        picture.addEventListener('load', function (event) {
//            document.getElementById('addPostImage').setAttribute('src', event.target.result);
//            document.getElementById('addPostImage').style.display = 'block';
//        });
//    }
//});


//// Create Status upload image
//document.getElementById('createStatusUrl').addEventListener('change', function () {
//    if (this.files[0]) {
//        var picture = new FileReader();
//        picture.readAsDataURL(this.files[0]);
//        picture.addEventListener('load', function (event) {
//            document.getElementById('createStatusImage').setAttribute('src', event.target.result);
//            document.getElementById('createStatusImage').style.display = 'block';
//        });
//    }
//});


//// create product upload image
//document.getElementById('createProductUrl').addEventListener('change', function () {
//    if (this.files[0]) {
//        var picture = new FileReader();
//        picture.readAsDataURL(this.files[0]);
//        picture.addEventListener('load', function (event) {
//            document.getElementById('createProductImage').setAttribute('src', event.target.result);
//            document.getElementById('createProductImage').style.display = 'block';
//        });
//    }
//});



// Set mode khi load trang
if (localStorage.theme === 'dark') {
    document.documentElement.classList.add('dark');
} else {
    document.documentElement.classList.remove('dark');
}

//function toggleDarkMode() {
//    const html = document.documentElement;
//    html.classList.toggle('dark');
//    localStorage.theme = html.classList.contains('dark') ? 'dark' : 'light';

//    updateThemeIcon();
//}

function toggleDarkMode() {
    const html = document.documentElement;
    const isDark = html.classList.contains('dark');

    if (isDark) {
        html.classList.remove('dark');
        localStorage.theme = 'light';
    } else {
        html.classList.add('dark');
        localStorage.theme = 'dark';
    }

    updateThemeIcon();
}

function updateThemeIcon() {    
    const icon = document.getElementById('darkModeIcon');
    if (!icon) return;

    const isDark = document.documentElement.classList.contains('dark');
    icon.setAttribute('name', isDark ? 'moon-outline' : 'sunny-outline');
}
