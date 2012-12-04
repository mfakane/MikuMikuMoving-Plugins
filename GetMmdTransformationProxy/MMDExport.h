typedef struct _D3DCOLORVALUE
{
	float r;
	float g;
	float b;
	float a;
} D3DCOLORVALUE;

typedef struct _D3DMATERIAL9
{
	D3DCOLORVALUE Diffuse;
	D3DCOLORVALUE Ambient;
	D3DCOLORVALUE Specular;
	D3DCOLORVALUE Emissive;
	float Power;
} D3DMATERIAL9;

typedef struct _D3DMATRIX
{
	union
	{
		struct
		{
			float _11, _12, _13, _14;
			float _21, _22, _23, _24;
			float _31, _32, _33, _34;
			float _41, _42, _43, _44;
		};
		float m[4][4];
	};
} D3DMATRIX;

#define _EXPORT

extern "C"
{
	_EXPORT float ExpGetFrameTime();
	_EXPORT int ExpGetPmdNum();
	_EXPORT char* ExpGetPmdFilename(int);
	_EXPORT int ExpGetPmdOrder(int);
	_EXPORT int ExpGetPmdMatNum(int);
	_EXPORT D3DMATERIAL9 ExpGetPmdMaterial(int, int);
	_EXPORT int ExpGetPmdBoneNum(int);
	_EXPORT char* ExpGetPmdBoneName(int, int);
	_EXPORT D3DMATRIX ExpGetPmdBoneWorldMat(int, int);
	_EXPORT int ExpGetPmdMorphNum(int);
	_EXPORT char* ExpGetPmdMorphName(int, int);
	_EXPORT float ExpGetPmdMorphValue(int, int);
	_EXPORT bool ExpGetPmdDisp(int);
	_EXPORT int ExpGetPmdID(int);
	_EXPORT int ExpGetAcsNum();
	_EXPORT int ExpGetPreAcsNum();
	_EXPORT char* ExpGetAcsFilename(int);
	_EXPORT int ExpGetAcsOrder(int);
	_EXPORT D3DMATRIX ExpGetAcsWorldMat(int);
	_EXPORT float ExpGetAcsX(int);
	_EXPORT float ExpGetAcsY(int);
	_EXPORT float ExpGetAcsZ(int);
	_EXPORT float ExpGetAcsRx(int);
	_EXPORT float ExpGetAcsRy(int);
	_EXPORT float ExpGetAcsRz(int);
	_EXPORT float ExpGetAcsSi(int);
	_EXPORT float ExpGetAcsTr(int);
	_EXPORT bool ExpGetAcsDisp(int);
	_EXPORT int ExpGetAcsID(int);
	_EXPORT int ExpGetAcsMatNum(int);
	_EXPORT D3DMATERIAL9 ExpGetAcsMaterial(int, int);
	_EXPORT int ExpGetCurrentObject();
	_EXPORT int ExpGetCurrentMaterial();
	_EXPORT int ExpGetCurrentTechnic();
	_EXPORT void ExpSetRenderRepeatCount(int);
	_EXPORT int ExpGetRenderRepeatCount();
}