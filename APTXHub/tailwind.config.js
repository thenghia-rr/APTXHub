module.exports = {
    darkMode: 'class',
    content: ['./Views/**/*.cshtml', './wwwroot/js/**/*.js'],
    safelist: [
        // BG colors
        {
            pattern: /^bg-(gray|red|blue|green|yellow|indigo|purple|pink)-(100|200|300|400|500|600|700|800|900)$/,
            variants: ['hover', 'focus', 'dark'],
        },
        // Text colors
        {
            pattern: /^text-(gray|red|blue|green|yellow|indigo|purple|pink)-(100|200|300|400|500|600|700|800|900)$/,
            variants: ['hover', 'focus', 'dark'],
        },
        // Border colors
        {
            pattern: /^border-(gray|red|blue|green|yellow|indigo|purple|pink)-(100|200|300|400|500|600|700|800|900)$/,
            variants: ['hover', 'focus', 'dark'],
        },
        // Padding
        {
            pattern: /^p[trblxy]?-(0|1|2|3|4|5|6|8|10|12|16|20|24|32|40|48|56|64)$/,
        },
        // Margin
        {
            pattern: /^m[trblxy]?-(0|1|2|3|4|5|6|8|10|12|16|20|24|32|40|48|56|64)$/,
        },
        // Width / Height
        { pattern: /^w-(\d+|full|screen)$/ },
        { pattern: /^h-(\d+|full|screen)$/ },
        // Typography
        { pattern: /^text-(xs|sm|base|lg|xl|2xl|3xl|4xl|5xl|6xl)$/, variants: ['hover', 'dark'] },
        { pattern: /^font-(sans|serif|mono)$/ },
        { pattern: /^leading-(none|tight|snug|normal|relaxed|loose)$/ },
        { pattern: /^tracking-(tighter|tight|normal|wide|wider|widest)$/ },
        { pattern: /^line-clamp-(1|2|3|4|5|6)$/ },
        // Layout & Flex
        { pattern: /^flex(-[a-z]+)?$/ },
        { pattern: /^items-(start|center|end|stretch|baseline)$/ },
        { pattern: /^justify-(start|center|end|between|around|evenly)$/ },
        { pattern: /^gap-(0|1|2|3|4|5|6|8|10|12|16|20|24|32)$/ },
        // Border / Rounded
        { pattern: /^rounded(-[a-z]+)?$/ },
        { pattern: /^border(-(0|2|4|8))?$/ },
        // Shadow
        { pattern: /^shadow(-(sm|md|lg|xl|2xl|inner|none))?$/ },
        // Grid
        { pattern: /^grid-cols-(1|2|3|4|5|6|7|8|9|10|11|12)$/ },
        // Position
        { pattern: /^(absolute|relative|fixed|sticky)$/ },
        { pattern: /^z-[0-9]+$/ },
        { pattern: /^top-[0-9]+$/ },
        { pattern: /^bottom-[0-9]+$/ },
        { pattern: /^left-[0-9]+$/ },
        { pattern: /^right-[0-9]+$/ },
        // Display
        { pattern: /^(hidden|block|inline|inline-block|flex|grid)$/ },
        // Overflow
        { pattern: /^overflow-(auto|hidden|visible|scroll)$/ },
        // Transition / Animation
        { pattern: /^transition(-(none|all|colors|opacity|shadow|transform))?$/ },
        { pattern: /^duration-(75|100|150|200|300|500|700|1000)$/ },
        { pattern: /^ease-(linear|in|out|in-out)$/ },
        { pattern: /^animate-(spin|ping|pulse|bounce|none)$/ }],
    theme: {
        extend: {},
    },
    plugins: [],
}
