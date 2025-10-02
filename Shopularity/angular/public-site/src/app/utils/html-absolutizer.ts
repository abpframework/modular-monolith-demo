import {environment} from "../../environments/environment";

export function absolutizeRelativeUrls(html: string): string {
  const root = environment.apis.default.url.replace(/\/+$/, ''); // sondaki /'ları temizle

  // src / href (tek seferde)
  html = html.replace(
    /(\s(?:src|href)=["'])([^"']+)(["'])/gi,
    (_m, pre, url, suf) => {
      const u = url.trim();
      if (/^(?:https?:|data:|blob:|\/\/)/i.test(u)) return `${pre}${u}${suf}`;
      if (u.startsWith('/')) return `${pre}${root}${u}${suf}`;
      return `${pre}${u}${suf}`; // ./foo.jpg gibi göreliyse dokunma (istersen burada da new URL kullanabilirsin)
    }
  );

  // srcset
  html = html.replace(
    /(\ssrcset=["'])([^"']+)(["'])/gi,
    (_m, pre, set, suf) => {
      const rewritten = set
        .split(',')
        .map(part => {
          const [url, descriptor] = part.trim().split(/\s+/, 2);
          if (/^(?:https?:|data:|blob:|\/\/)/i.test(url)) return part.trim();
          const abs = url.startsWith('/') ? `${root}${url}` : url;
          return descriptor ? `${abs} ${descriptor}` : abs;
        })
        .join(', ');
      return `${pre}${rewritten}${suf}`;
    }
  );

  return html;
}
