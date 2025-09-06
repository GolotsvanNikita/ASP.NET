document.addEventListener('DOMContentLoaded', function ()
{
    document.querySelectorAll('.xp-player').forEach(player =>
    {
        const audio = player.querySelector('audio');
        const playPauseBtn = player.querySelector('.xp-play-pause');
        const seekSlider = player.querySelector('.xp-seek-slider');
        const timeDisplay = player.querySelector('.xp-time');

        playPauseBtn.addEventListener('click', () =>
        {
            if (audio.paused)
            {
                audio.play();
                playPauseBtn.textContent = document.cookie == 'lang=uk' ? 'Пауза' : 'Pause';
                playPauseBtn.setAttribute('data-action', 'Pause');
            }
            else
            {
                audio.pause();
                playPauseBtn.textContent = document.cookie == 'lang=uk' ? 'Увімкнути' : 'Play';
                playPauseBtn.setAttribute('data-action', 'Play');
            }
        });

        audio.addEventListener('timeupdate', () =>
        {
            const currentTime = audio.currentTime;
            const duration = audio.duration || 0;
            const percentage = duration ? (currentTime / duration) * 100 : 0;
            seekSlider.value = percentage;

            const formatTime = (seconds) =>
            {
                const min = Math.floor(seconds / 60);
                const sec = Math.floor(seconds % 60);
                return `${min.toString().padStart(2, '0')}:${sec.toString().padStart(2, '0')}`;
            };
            timeDisplay.textContent = `${formatTime(currentTime)} / ${formatTime(duration)}`;
        });

        seekSlider.addEventListener('input', () =>
        {
            const duration = audio.duration || 0;
            audio.currentTime = (seekSlider.value / 100) * duration;
        });
    });
});