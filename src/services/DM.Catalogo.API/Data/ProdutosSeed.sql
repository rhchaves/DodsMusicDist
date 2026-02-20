USE DodsMusicDev;
GO

INSERT INTO dbo.Produtos
(
    Id,
    Nome,
    Descricao,
    Ativo,
    Valor,
    DataCadastro,
    Imagem,
    QuantidadeEstoque
)
VALUES
(
    '66be4d75-a8a9-4087-b264-be11bae37d9e',
    'Contra Baixo Fender Deluxe Jazz Bass',
    'O contrabaixo Fender Deluxe Active Jazz Bass V tem a sonoridade clássica do Jazz Basss, porém com um mix de características modernas, como o eq. ativo de 3 bandas. Com um par de captadores Noiseless Jazz Bass V e controle de pan, você consegue uma enorme gama de sonoridades. O eq de 3 bandas tem agudo ( + /- 10 db em 8kHz), médio ( + 10dB, -15dB em 500Hz) e grave ( + /- 12db em 40Hz), além disso acompanha um luxuoso Deluxe Gig Bag.',
    1,
    11000.00,
    GETDATE(),
    'f7a42f39-349e-4cf2-988d-1c1e2b3d886e_Contra-Baixo-Fender-Deluxe.jpg',
    21
),
(
    'd6a7f6b5-5ab6-4a06-8f8e-85e9d8cd4122',
    'Guitarra Les Paul Black',
    'Guitarra Strinberg lps-230 bks - A Guitarra LPS-230 BKS da Strinberg, é um instrumento com o corpo construído em Basswood Sólido, braço em Maple, escala em Technical Wood 24,75’’, tampo em Basswood e seu formato sendo o Les Paul, além de contar com 22 trastes. Possui o acabamento fosco de seu corpo a cor black satin realçam a beleza desse instrumento, sendo esse uma excelente escolha para quem procura um instrumento de alta qualidade, sendo projetado para oferecer um som encorpado e uma tocabilidade confortável.',
    1,
    1500.00,
    GETDATE(),
    '88905c40-d364-45b9-af57-dceb1e715eca_guitarra_les_paul_black.jpg',
    10
),
(
    '2d55bfc5-09f6-47e3-8c35-4a0af06fa8b4',
    'Baixo Square',
    'Baixo Square 5 Cordas Natural Jazz Bass descrição completa',
    1,
    4000.00,
    GETDATE(),
    '28901bbb-bd8e-425e-93db-d57b12eba1cb_baixo-square-natural.jpg',
    9
),
(
    '5dde764b-e9f5-4e64-9545-25cf1d36a0a5',
    'Bateria DW Collectors Pure Maple Pale Blue',
    'Bateria DW Original Americana, Nova nunca saiu de casa! Somente para Venda. Dispenso Curiosos e perus! Qualquer dúvida é só me perguntar. Vídeos e o som da bateria no meu YT canal. à venda somente Tambores.',
    1,
    40000.00,
    GETDATE(),
    'af231c4c-d3c9-48da-aae1-87718482510c_Bateria-DW-Collectors-Pale-Blue-Oyster.jpg',
    5
),




(
    '8a1d1a9e-9b8c-4d5b-9d8b-3c7e1d5a1a01',
    'Teclado Yamaha PSR-E373',
    'Teclado arranjador portátil com 61 teclas sensitivas, motor sonoro AWM Stereo Sampling e mais de 600 vozes de alta qualidade.',
    1,
    2450.00,
    GETDATE(),
    'b1cf36cc-a934-467b-80d0-e7f24a188e59_Teclado Yamaha PSR-E373.jpg',
    18
),
(
    '2c3b9f24-7d9a-4d9c-a3b7-9f1c2d7a2b02',
    'Piano Digital Casio Privia PX-S1100',
    'Piano digital compacto com teclas com ação de martelo, timbres realistas e conectividade Bluetooth.',
    1,
    6200.00,
    GETDATE(),
    '15e966cf-ed5a-4254-86b3-255dd5388db6_Piano Digital Casio Privia PX-S1100.jpg',
    18
),
(
    'f4d7a2b1-6c9e-4b9f-8a2d-7c1e9a3c3c03',
    'Violão Yamaha F310',
    'Violão acústico folk com tampo em spruce, ideal para iniciantes e músicos intermediários.',
    1,
    950.00,
    GETDATE(),
    '1634b535-cf4a-4799-84b1-92dc28760144_Violão Yamaha F310.jpg',
    15
),
(
    'b8a7c6d5-4e3f-42a1-9c7d-8b1e4d5a4d04',
    'Violino Eagle VE441 4/4',
    'Violino acústico tamanho 4/4 indicado para estudantes, com acabamento tradicional e som equilibrado.',
    1,
    780.00,
    GETDATE(),
    'b8fdd985-38a0-4c70-be2d-66f4f53eb48e_Violino Eagle VE441 4-4.jpg',
    15
),
(
    '9d3b8a2f-1c7e-4f2b-9a8d-5c6e7f8a5e05',
    'Saxofone Alto Michael WASM45N',
    'Saxofone alto em Mi bemol, construção em latão laqueado e timbre encorpado para estudo e apresentações.',
    1,
    4200.00,
    GETDATE(),
    'f9512791-1b07-42a9-b058-ff9c5f228d78_Saxofone Alto Michael WASM45N.jpg',
    16
),
(
    '1a2b3c4d-5e6f-4789-9a1b-2c3d4e5f6f06',
    'Cajón Inclinado FSA',
    'Cajón acústico com excelente resposta de grave e esteira interna ajustável.',
    1,
    890.00,
    GETDATE(),
    '98db116c-2051-4a30-834c-b8816ac296c1_Cajón Inclinado FSA.jpg',
    14
),
(
    '7f6e5d4c-3b2a-4987-8c9d-1e2f3a4b7a07',
    'Pedal de Efeito Boss DS-1 Distortion',
    'Pedal de distorção clássico com timbre encorpado e sustain prolongado.',
    1,
    620.00,
    GETDATE(),
    'e36f2b00-bfcc-4f25-80ea-c7b933dad30c_Pedal de Efeito Boss DS-1 Distortion.jpg',
    19
),
(
    'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b8b08',
    'Pedaleira Zoom G1X Four',
    'Pedaleira multiefeitos com pedal de expressão, ideal para guitarristas que buscam versatilidade.',
    1,
    980.00,
    GETDATE(),
    '6b065eee-cdcd-4ad8-9f0c-75612bd19d11_Pedaleira Zoom G1X Four.jpg',
    19
),
(
    'c9b8a7d6-e5f4-4321-9a8b-7c6d5e4f9c09',
    'Interface de Áudio Focusrite Scarlett 2i2',
    'Interface USB com dois pré-amplificadores de microfone de alta qualidade e baixa latência.',
    1,
    1850.00,
    GETDATE(),
    'f17b2585-ad0f-4d35-9913-07fa62b9bcfb_Interface de Áudio Focusrite Scarlett 2i2.jpg',
    20
),
(
    '4f3e2d1c-b9a8-4c7d-9e1f-2a3b4c5d0d10',
    'Microfone Shure SM58',
    'Microfone dinâmico cardioide, padrão da indústria para vocais ao vivo.',
    1,
    780.00,
    GETDATE(),
    '3b9c1bea-f85f-45a4-a196-02d5636d55b9_Microfone Shure SM58.jpg',
    20
),
(
    'e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f1e11',
    'Suporte para Teclado em X',
    'Suporte dobrável e ajustável, compatível com a maioria dos teclados.',
    1,
    180.00,
    GETDATE(),
    '6db963e2-9c0d-473c-aa62-65ccaf994142_Suporte para Teclado em X.jpg',
    13
),

(
    '0a1b2c3d-4e5f-4689-9a7b-6c5d4e3f2f12',
    'Cabo P10 Mono 3m Santo Ângelo',
    'Cabo de áudio profissional com conectores P10 e excelente blindagem contra ruídos.',
    1,
    95.00,
    GETDATE(),
    '1712e43a-ec2d-4295-b352-8fcab88dbd21_Cabo P10 Mono 3m Santo Ângelo.jpg',
    13
);
GO
